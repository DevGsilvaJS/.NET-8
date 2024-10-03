using System.Text;

namespace ChatGS.Querys
{
    public class UsuarioQuery
    {
        public string ListAllUsers()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT usu.USUID, usu.USULOGIN, usu.USUSENHA, pes.PESID, pes.PESNOME, pes.PESSOBRENOME ");
            sb.AppendLine("FROM TB_USU_USUARIO usu ");
            sb.AppendLine("JOIN TB_PES_PESSOA pes ON pes.PESID = usu.PESID");

            return sb.ToString();
        }

        public string GetUserById()
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"
        SELECT 
            USU.USUID,
            USU.USULOGIN,
            USU.USUSENHA,
            PES.PESID,
            PES.PESNOME,
            PES.PESSOBRENOME,
            PES.PESDATACADASTRO
        FROM TB_PES_PESSOA PES
        JOIN TB_USU_USUARIO USU ON USU.PESID = PES.PESID
        WHERE USU.PESID = @PESID");

            return sb.ToString();
        }
    }
}
