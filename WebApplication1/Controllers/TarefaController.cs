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
    public class TarefaController : ControllerBase
    {
        private readonly MeuDbContext _context;

        public TarefaController(MeuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Tarefa>> GetTarefas()
        {
            return _context.Tarefa.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Tarefa> GetTarefa(int id)
        {
            var tarefa = _context.Tarefa.Find(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return tarefa;
        }

        [HttpPost]
        public ActionResult<Tarefa> PostTarefa(Tarefa tarefa)
        {
            // Obtenha o projeto ao qual a tarefa será adicionada
            var projeto = _context.Projeto.Find(tarefa.ProjetoId);

            if (projeto == null)
            {
                return BadRequest("Projeto não encontrado.");
            }

            // Verifique se o projeto atingiu o limite de 20 tarefas
            if (projeto.Tarefas.Count >= 20)
            {
                return BadRequest("O projeto atingiu o limite máximo de 20 tarefas.");
            }

            // Validar a prioridade antes de adicionar a tarefa
            if (string.IsNullOrEmpty(tarefa.Prioridade) ||
                !new[] { "baixa", "média", "alta" }.Contains(tarefa.Prioridade.ToLower()))
            {
                return BadRequest("Prioridade inválida. Deve ser 'baixa', 'média' ou 'alta'.");
            }

            // Adicione a tarefa ao projeto
            projeto.Tarefas.Add(tarefa);

            // Salve as alterações no banco de dados
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTarefa), new { id = tarefa.Id }, tarefa);
        }

        [HttpPost("{id}/comentarios")]
        public IActionResult AdicionarComentario(int id, [FromBody] Comentario comentario)
        {
            var tarefa = _context.Tarefa.Include(t => t.Comentarios).FirstOrDefault(t => t.Id == id);

            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada.");
            }

            // Adiciona o novo comentário à tarefa
            comentario.TarefaId = id;
            comentario.DataCriacao = DateTime.Now;
            tarefa.Comentarios.Add(comentario);

            // Adiciona o comentário ao histórico de atualizações
            _context.HistoricoAtualizacao.Add(new HistoricoAtualizacao
            {
                TarefaId = id,
                CampoModificado = "Comentario",
                ValorAnterior = null,
                ValorAtual = comentario.Conteudo,
                DataModificacao = comentario.DataCriacao,
                UsuarioId = 1 // Substitua pelo ID do usuário ativo
            });

            _context.SaveChanges();

            return Ok(comentario);
        }

        [HttpPut("{id}")]
        public IActionResult PutTarefa(int id, Tarefa tarefa)
        {
            if (id != tarefa.Id)
            {
                return BadRequest();
            }

            // Obtenha a tarefa existente no banco de dados
            var tarefaExistente = _context.Tarefa.Find(id);

            if (tarefaExistente == null)
            {
                return NotFound();
            }

            // Compare os valores das propriedades e registre as alterações no histórico
            var alteracoes = new List<HistoricoAtualizacao>();

            if (tarefaExistente.Titulo != tarefa.Titulo)
            {
                alteracoes.Add(CriarHistorico(tarefaExistente.Id, "Titulo", tarefaExistente.Titulo, tarefa.Titulo));
                tarefaExistente.Titulo = tarefa.Titulo;
            }

            if (tarefaExistente.Descricao != tarefa.Descricao)
            {
                alteracoes.Add(CriarHistorico(tarefaExistente.Id, "Descricao", tarefaExistente.Descricao, tarefa.Descricao));
                tarefaExistente.Descricao = tarefa.Descricao;
            }

            if (tarefaExistente.DataVencimento != tarefa.DataVencimento)
            {
                alteracoes.Add(CriarHistorico(tarefaExistente.Id, "DataVencimento",
                                              tarefaExistente.DataVencimento?.ToString(),
                                              tarefa.DataVencimento?.ToString()));
                tarefaExistente.DataVencimento = tarefa.DataVencimento;
            }

            if (tarefaExistente.Status != tarefa.Status)
            {
                alteracoes.Add(CriarHistorico(tarefaExistente.Id, "Status", tarefaExistente.Status, tarefa.Status));
                tarefaExistente.Status = tarefa.Status;
            }

            if (tarefaExistente.Prioridade != tarefa.Prioridade)
            {
                alteracoes.Add(CriarHistorico(tarefaExistente.Id, "Prioridade", tarefaExistente.Prioridade, tarefa.Prioridade));
                tarefaExistente.Prioridade = tarefa.Prioridade;
            }

            // Salve as alterações no histórico e na tarefa no banco de dados
            _context.HistoricoAtualizacao.AddRange(alteracoes);
            _context.SaveChanges();

            return NoContent();
        }
        private WebApplication1.Class.HistoricoAtualizacao CriarHistorico(int tarefa, string campo, string valorAnterior, string valorAtual)
        {
            return new WebApplication1.Class.HistoricoAtualizacao
            {
                TarefaId = tarefa,  // Especificando explicitamente a propriedade TarefaId da classe HistoricoAtualizacao
                CampoModificado = campo,
                ValorAnterior = valorAnterior,
                ValorAtual = valorAtual,
                DataModificacao = DateTime.Now,
                UsuarioId = 1 // Substitua pelo ID do usuário ativo
            };
        }



        [HttpDelete("{id}")]
        public ActionResult<Tarefa> DeleteTarefa(int id)
        {
            var tarefa = _context.Tarefa.Find(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            _context.Tarefa.Remove(tarefa);
            _context.SaveChanges();

            return tarefa;
        }
    }
}
