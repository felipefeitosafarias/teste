using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Class;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        private readonly MeuDbContext _context;

        public ProjetoController(MeuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Projeto>> GetProjetos()
        {
            return _context.Projeto.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Projeto> GetProjeto(int id)
        {
            var projeto = _context.Projeto.Find(id);

            if (projeto == null)
            {
                return NotFound();
            }

            return projeto;
        }

        [HttpPost]
        public ActionResult<Projeto> PostProjeto(Projeto projeto)
        {
            _context.Projeto.Add(projeto);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProjeto), new { id = projeto.Id }, projeto);
        }

        [HttpPut("{id}")]
        public IActionResult PutProjeto(int id, Projeto projeto)
        {
            if (id != projeto.Id)
            {
                return BadRequest();
            }

            _context.Entry(projeto).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Projeto.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Projeto> DeleteProjeto(int id)
        {
            var projeto = _context.Projeto.Include(p => p.Tarefas).FirstOrDefault(p => p.Id == id);

            if (projeto == null)
            {
                return NotFound();
            }

            if (projeto.Tarefas.Any(t => t.Status == "pendente"))
            {
                return BadRequest("Não é possível excluir o projeto. Existem tarefas pendentes associadas a ele. Conclua ou remova as tarefas primeiro.");
            }

            _context.Projeto.Remove(projeto);
            _context.SaveChanges();

            return projeto;
        }
    }
}
