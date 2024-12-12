namespace BookManagerAPI.Models
{

    public class Usuario
    {
        public int Id { get; set; }
        public long Telefone { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
    }
}   