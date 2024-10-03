namespace ChatGS.ResponseModel
{
    public class ResponseModel<T>
    {
        public object Data { get; set; } // Especifica que o Data é do tipo T
        public string Mensagem { get; set; } = string.Empty;
        public bool Success { get; set; } // Indicador de sucesso
    }

}
