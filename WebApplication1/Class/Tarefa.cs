using System;
using System.Collections.Generic;

namespace WebApplication1.Class
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public string Status { get; set; }
        public string Prioridade { get; set; }
        public int ProjetoId { get; set; }  // Referência ao projeto ao qual a tarefa pertence

        // Propriedades de navegação
        public Projeto Projeto { get; set; }
        public List<HistoricoAtualizacao> HistoricoAtualizacoes { get; set; } = new List<HistoricoAtualizacao>();  // Adicione esta linha para representar o relacionamento com o histórico de atualizações
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }


}
