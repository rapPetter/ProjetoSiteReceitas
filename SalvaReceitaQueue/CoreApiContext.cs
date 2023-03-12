using SalvaReceitaQueue.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalvaReceitaQueue
{
    public class CoreApiContext  : DbContext
    {
        public DbSet<Receita> Receitas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SiteReceitas;Trusted_Connection=True;persist security info=True;");
        }
       
        
    }
}