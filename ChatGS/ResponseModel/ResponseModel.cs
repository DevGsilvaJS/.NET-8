namespace ChatGS.ResponseModel
{
    public class ResponseModel<T>
    {

        public T Dados { get; set; }
        public string Mensagem { get; set; } = string.Empty;
    }
}
