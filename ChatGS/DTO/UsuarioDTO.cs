namespace ChatGS.DTO
{
    public class UsuarioDTO
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public PessoaDTO Pessoa { get; set; } = new PessoaDTO();
        public GrupoUsuariosDTO GrupoUsuarios { get; set; } = new GrupoUsuariosDTO();

    }
}
