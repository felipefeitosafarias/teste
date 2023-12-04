using System;

namespace WebApplication1.Class
{
    public class HistoricoAtualizacao
    {
        public int Id { get; set; }
        public int TarefaId { get; set; }
        public string CampoModificado { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorAtual { get; set; }
        public DateTime DataModificacao { get; set; }
        public int UsuarioId { get; set; }

        // Propriedades de navegação
        public Tarefa Tarefa { get; set; }
        public Usuario Usuario { get; set; }
    }
}
