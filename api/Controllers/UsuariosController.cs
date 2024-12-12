using BookManagerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private static List<Usuario> usuarios = new();

        [HttpGet]
        public ActionResult<List<Usuario>> VerUsuarios()
        {
            return Ok(usuarios);
        }

        [HttpPost]
        public ActionResult AdicionarUsuario(Usuario novoUsuario)
        {
            if (string.IsNullOrEmpty(novoUsuario.Nome) || novoUsuario.Telefone <= 0 || novoUsuario.Idade <= 0)
            {
                return BadRequest("Dados inválidos. Por favor, preencha todos os campos corretamente.");
            }

            novoUsuario.Id = usuarios.Count > 0 ? usuarios[usuarios.Count - 1].Id + 1 : 1;
            usuarios.Add(novoUsuario);
            return Ok(novoUsuario);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletarUsuario(int id)
        {
            var usuario = usuarios.FirstOrDefault(usuario => usuario.Id == id);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            usuarios.Remove(usuario);
            return NoContent();
        }
    }
}
