using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalvaReceitaQueue.Models
{
    public class Usuario
    {
        public int Id {get; set;}
        public string Email {get; set;}
        public string Cidade {get; set;}
        public int QuantidadeDeReceitasPostadas {get; set;}
    }
}