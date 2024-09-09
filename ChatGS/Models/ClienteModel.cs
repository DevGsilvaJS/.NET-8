namespace ChatGS.Models
{
    public class ClienteModel
    {
        public int CLIID { get; set; }
        public int PESID { get; set; }
        public DateTime CLIDATANASCIMENTO { get; set; }
        public char CLISEXO { get; set; }
        public PessoaModel TB_PES_PESSOA { get; set; }


        public ClienteModel()
        {

        }

        public ClienteModel(char CLISEXO) : this()
        {
            if (CLISEXO != 'M' && CLISEXO != 'F')
                throw new ArgumentException("Sexo obrigatório!", nameof(CLISEXO));

            this.CLISEXO = CLISEXO;
        }
    }
}
