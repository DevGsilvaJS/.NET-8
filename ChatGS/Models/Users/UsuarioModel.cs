using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ChatGS.Models.Users
{

    [Table("TB_USU_USUARIO")]
    public class UsuarioModel
    {
        public int USUID { get; set; }
        public int PESID { get; set; }
        public int GUSID { get; set; }
        public string USULOGIN { get; set; }
        public string USUSENHA { get; set; }
        public string USUEMAIL { get; set; }

        public PessoaModel Pessoa { get; set; } = new PessoaModel();

        public string RetornaUsuarioPorID()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT");
            sb.AppendLine("     USU.USUID,                                                 ");
            sb.AppendLine(" 	USU.PESID,                                                 ");
            sb.AppendLine(" 	USU.GUSID,                                                 ");
            sb.AppendLine(" 	USU.USULOGIN,                                              ");
            sb.AppendLine(" 	USU.USUSENHA,                                              ");
            sb.AppendLine(" 	PES.PESID,                                                 ");
            sb.AppendLine(" 	PES.PESNOME,                                               ");
            sb.AppendLine(" 	PES.PESSOBRENOME,                                          ");
            sb.AppendLine(" 	PES.PESDOCFEDERAL,                                         ");
            sb.AppendLine(" 	PES.PESDOCESTADUAL,                                        ");
            sb.AppendLine(" 	PES.PESDATACADASTRO,                                       ");
            sb.AppendLine(" 	GUS.GUSID,                                                 ");
            sb.AppendLine(" 	GUS.GUSDESCRICAO                                           ");
            sb.AppendLine(" FROM TB_USU_USUARIO USU                                        ");
            sb.AppendLine(" JOIN TB_PES_PESSOA PES ON PES.PESID = USU.PESID       ");
            sb.AppendLine(" JOIN TB_GUS_GRUPOUSUARIO GUS ON GUS.GUSID = USU.GUSID ");



            return sb.ToString();
        }

    }
}
