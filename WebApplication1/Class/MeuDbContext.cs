using Microsoft.EntityFrameworkCore;
using WebApplication1.Class;
using WebApplication1.Controllers;

public class MeuDbContext : DbContext
{
    public MeuDbContext(DbContextOptions<MeuDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Projeto> Projeto { get; set; }
    public DbSet<Tarefa> Tarefa { get; set; }
    public DbSet<HistoricoAtualizacao> HistoricoAtualizacao { get; set; }
    public DbSet<Comentario> Comentario { get; set; }
    public DbSet<RelatorioDesempenho> RelatorioDesempenho { get; set; }
}

