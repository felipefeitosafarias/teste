namespace WebApplication1.Class
{
    public class Comentario
    { public int Id { get; set; }
        public int TarefaId { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataCriacao { get; set; }

        // Propriedades de navegação
        public Tarefa Tarefa { get; set; }
    }
}
