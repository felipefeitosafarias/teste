using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApplication1.Class;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "gerente")]
    public class RelatorioController : ControllerBase
    {
        private readonly MeuDbContext _context;

        public RelatorioController(MeuDbContext context)
        {
            _context = context;
        }

        [HttpGet("media-tarefas-concluidas-por-usuario")]
        public ActionResult<double> GetMediaTarefasConcluidasPorUsuario()
        {
            var dataLimite = DateTime.Now.AddDays(-30);

            var mediaTarefasConcluidas = _context.HistoricoAtualizacao
                .Where(h => h.CampoModificado == "Status" &&
                            h.ValorAnterior == "em andamento" &&
                            h.ValorAtual == "concluída" &&
                            h.DataModificacao >= dataLimite)
                .GroupBy(h => h.UsuarioId)
                .Select(g => new
                {
                    UsuarioId = g.Key,
                    MediaTarefasConcluidas = g.Count()
                })
                .Average(r => r.MediaTarefasConcluidas);

            return Ok(mediaTarefasConcluidas);
        }
    }
}
