namespace ChatGS.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
        public int IdPessoa { get; set; }
        public PessoaModel Pessoa { get; set; }

    }
}
