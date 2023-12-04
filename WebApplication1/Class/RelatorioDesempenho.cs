namespace WebApplication1.Class
{
    public class RelatorioDesempenho
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal NumeroMedioTarefasConcluidas { get; set; }
        public DateTime DataRelatorio { get; set; }

        // Propriedades de navegação
        public Usuario Usuario { get; set; }
    }
}
