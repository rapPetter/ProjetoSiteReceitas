using coreApi.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace coreApi.Context
{
    public class CoreApiContext:DbContext
    {
        public CoreApiContext(DbContextOptions<CoreApiContext> options) : base(options) { }
       // public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<Receita> Receitas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
