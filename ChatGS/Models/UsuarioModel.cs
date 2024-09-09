namespace ChatGS.Models
{
    public class UsuarioModel
    {
        public int? USUID { get; set; }
        public int PESID { get; set; }
        public int GUSID { get; set; }
        public string USULOGIN { get; set; } 
        public string USUSENHA { get; set; }

        public PessoaModel Pessoa { get; set; } = new PessoaModel();
        public GrupoUsuarioModel GrupoUsuarios { get; set; } = new GrupoUsuarioModel();

    }
}
