using ChatGS.Models;

namespace ChatGS.DTO
{
    public class ClienteDTO
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public int IdGrupoUsuarios { get; set; }

    }
}
