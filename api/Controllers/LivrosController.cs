using BookManagerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private static List<Livro> books = new()
        {
            new() { Id = 1, Titulo = "Dom Camusro", Autor = "Machado de Assis", Ano = 1899, Quantidade = 2 },
            new() { Id = 2, Titulo = "Memórias Póstumas", Autor = "Machado de Assis", Ano = 1881, Quantidade = 3 },
            new() { Id = 3, Titulo = "Grande Sertão Veredas", Autor = "João Guimarães Rosa", Ano = 1956, Quantidade = 4 },
            new() { Id = 4, Titulo = "O Cortiço", Autor = "Aluísio Azevedo", Ano = 1890, Quantidade = 4 },
            new() { Id = 5, Titulo = "Iracema", Autor = "José de Alencar", Ano = 1865, Quantidade = 1 },
            new() { Id = 6, Titulo = "Macunaíma", Autor = "Mário de Andrade", Ano = 1928, Quantidade = 11 },
            new() { Id = 7, Titulo = "Capitães de Areia", Autor = "Jorge Amado", Ano = 1937, Quantidade = 2 },
            new() { Id = 8, Titulo = "Vidas Secas", Autor = "Graciliano Ramos", Ano = 1938, Quantidade = 9 },
            new() { Id = 9, Titulo = "A Moreninha", Autor = "Joaquim Manuel de Macedo", Ano = 1844, Quantidade = 2 },
            new() { Id = 10, Titulo = "O Tempo e o Vento", Autor = "Érico Veríssimo", Ano = 1949, Quantidade = 1 },
            new() { Id = 11, Titulo = "A Hora da Estrela", Autor = "Clarice Lispector", Ano = 1977, Quantidade = 2 },
            new() { Id = 12, Titulo = "O Quinze", Autor = "Rachel de Queiroz", Ano = 1930, Quantidade = 1 },
            new() { Id = 13, Titulo = "Menino do Engenho", Autor = "José Lins do Rego", Ano = 1932, Quantidade = 5 },
            new() { Id = 14, Titulo = "Sagarana", Autor = "João Guimarães Rosa", Ano = 1946, Quantidade = 3 },
            new() { Id = 15, Titulo = "Fogo Morto", Autor = "José Lins do Rego", Ano = 1943, Quantidade = 3 }
        };

        [HttpGet]
        public ActionResult<List<Livro>> VerLivros() => Ok(books);

        [HttpPost]
        public ActionResult AdicionarLivro(Livro newBook)
        {
            if (string.IsNullOrEmpty(newBook.Autor) || string.IsNullOrEmpty(newBook.Titulo) || newBook.Ano <= 0 || newBook.Quantidade <= 0)
                return BadRequest("Dados inválidos. Por favor, preencha todos os campos corretamente.");

            newBook.Id = books.Any() ? books[^1].Id + 1 : 1;
            books.Add(newBook);
            return Ok(newBook);
        }

        [HttpPut("{livroId}/emprestar/{usuarioId}")]
        public ActionResult EmprestarLivro(int livroId, int usuarioId)
        {
            var livro = books.FirstOrDefault(l => l.Id == livroId);
            if (livro == null) return NotFound("Livro não encontrado.");
            if (livro.Quantidade == 0) return BadRequest("Todas as unidades do livro já foram emprestadas.");

            livro.Quantidade--;
            livro.UsuariosEmprestados.Add(usuarioId);
            livro.QuantidadeEmprestada++;
            return Ok($"Livro {livro.Titulo} emprestado para o usuário de ID:{usuarioId}. Quantidade emprestada: {livro.QuantidadeEmprestada}/{livro.Quantidade}");
        }

        [HttpPut("{livroId}/devolver/{usuarioId}")]
        public ActionResult DevolverLivro(int livroId, int usuarioId)
        {
            var livro = books.FirstOrDefault(l => l.Id == livroId);
            if (livro == null) return NotFound("Livro não encontrado.");
            if (livro.QuantidadeEmprestada == 0) return BadRequest("Nenhuma unidade do livro está emprestada.");

            livro.Quantidade++;
            livro.UsuariosEmprestados.Remove(usuarioId);
            livro.QuantidadeEmprestada--;
            return Ok($"O livro {livro.Titulo} devolvido pelo usuário de ID:{usuarioId}. Quantidade emprestada: {livro.QuantidadeEmprestada}/{livro.Quantidade}");
        }

        [HttpGet("emprestados")]
        public ActionResult LivrosEmprestados()
        {
            var livrosEmprestados = books
                .Where(l => l.QuantidadeEmprestada > 0)
                .Select(l => new { l.Id, l.Titulo, l.Autor, l.QuantidadeEmprestada, l.Quantidade })
                .ToList();

            return livrosEmprestados.Any() ? Ok(livrosEmprestados) : BadRequest("Não há livros emprestados.");
        }

        [HttpDelete("{id}")]
        public ActionResult RemoverLivro(int id)
        {
            var livro = books.FirstOrDefault(l => l.Id == id);
            if (livro == null) return NotFound("Livro não encontrado.");
            if (livro.Quantidade <= 0) return BadRequest("Não há unidades disponíveis para remover.");

            livro.Quantidade--;
            if (livro.Quantidade == 0) books.Remove(livro);

            return Ok(books);
        }

        [HttpDelete("remover-livro/{id}")]
        public ActionResult RemoverLivroInteiro(int id)
        {
            var livro = books.FirstOrDefault(l => l.Id == id);
            if (livro == null) return NotFound("Livro não encontrado.");

            books.Remove(livro);
            return Ok($"O livro com ID {id} foi removido completamente.");
        }
    }
}
