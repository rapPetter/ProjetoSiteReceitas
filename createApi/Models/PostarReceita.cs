namespace createApi.Models
{
    public class PostarReceita
    {
        public string Email { get; set; }
        public string NomeReceita { get; set; }
        public int TempoPreparo { get; set; }
        public int Porcoes { get; set; }
        public string Categoria { get; set; }
        public string Ingredientes { get; set; }
        public string Descricao { get; set; }
        public string? Ipv4 { get; set; }
        public DateTime DataPublicacao { get; set; } = DateTime.Now;

    }
}
