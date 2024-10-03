namespace ChatGS.Models.Users
{
    public class TelefoneModel
    {
        public int TELID { get; set; }
        public int PESID { get; set; }
        public string TELDDD { get; set; }
        public string TELCELULAR { get; set; }

        public TelefoneModel(string telddd, string telcelular)
        {

            if (string.IsNullOrEmpty(telddd) || telddd.Length != 2)
                throw new ArgumentException("O DDD deve ter exatamente 2 posições.", nameof(telddd));

            if (string.IsNullOrEmpty(telcelular) || telcelular.Length != 9)
                throw new ArgumentException("O número do celular deve ter exatamente 9 posições.", nameof(telcelular));

            TELDDD = telddd;
            TELCELULAR = telcelular;
        }
    }
}
