namespace ChatGS.Models
{
    public class PessoaModel
    {
        public int? PESID { get; set; }
        public string PESNOME { get; set; } = string.Empty;
        public string PESSOBRENOME { get; set; } = string.Empty;
        public int PESDOCFEDERAL { get; set; }
        public int PESDOCESTADUAL { get; set; }
        public DateTime PESDATACADASTRO { get; set; } = DateTime.Now;

    }
}
