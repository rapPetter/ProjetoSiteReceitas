using System.ComponentModel.DataAnnotations;

namespace coreApi.Models
{
    public class Usuario
    {
        [Key]
        public int Id {get; set;}
        public string Email {get; set;}
        public string Cidade {get; set;}
        public int QuantidadeDeReceitasPostadas {get; set;}
    }
}