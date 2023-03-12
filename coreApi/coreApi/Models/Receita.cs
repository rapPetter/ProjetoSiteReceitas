using coreApi.Enum;
using System.ComponentModel.DataAnnotations;

namespace coreApi.Models
{
    public class Receita
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string NomeReceita { get; set; }
        public string Ingredientes { get; set; }
        public int TempoPreparo { get; set; }
        public int Porcoes { get; set; }
        public CategoriaEnum? Categoria { get; set; }
        public string Descricao { get; set; }
        public DateTime DataPublicacao { get; set; }
        public int? Caloria { get; set; }
        public int Status { get; set; }



    }
}
