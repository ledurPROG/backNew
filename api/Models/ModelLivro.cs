namespace BookManagerAPI.Models
{









    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int Ano { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadeEmprestada { get; set; }
        public bool Emprestado => QuantidadeEmprestada > 0;
        public List<int> UsuariosEmprestados { get; set; } = new List<int>();
    }
}
