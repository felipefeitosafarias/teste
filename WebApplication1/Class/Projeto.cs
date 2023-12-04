using System.Collections.Generic;

namespace WebApplication1.Class
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int UsuarioId { get; set; }  // Referência ao usuário que criou o projeto

        // Propriedades de navegação
        public Usuario Usuario { get; set; }
        public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();  // Adicione esta linha para representar o relacionamento com Tarefas
    }
}
