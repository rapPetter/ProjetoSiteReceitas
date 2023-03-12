using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalvaReceitaQueue.Models
{
    public class ReceitaViewModel
    {
        public string Email { get; set; }
        public string NomeReceita { get; set; }
        public string Ingredientes { get; set; }
        public int TempoPreparo { get; set; }
        public int Porcoes { get; set; }
        public string Descricao { get; set; }
        public string? Ipv4 { get; set; }
        public DateTime? DataPublicacao { get; set; }

    }
}