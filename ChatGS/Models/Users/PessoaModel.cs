using System.ComponentModel.DataAnnotations.Schema;

namespace ChatGS.Models.Users
{

    [Table("TB_PES_PESSOA")]
    public class PessoaModel
    {
        public int PESID { get; set; }
        public string PESNOME { get; set; } = string.Empty;
        public string PESSOBRENOME { get; set; } = string.Empty;
        public DateTime PESDATACADASTRO { get; set; } = DateTime.Now;

    }
}
