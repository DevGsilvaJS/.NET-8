namespace ChatGS.Interfaces
{
    public interface IChat
    {
        Task<bool> IniciarConversa(int usuarioID);
        Task<bool> EnviarMensagem(int conid, int usuid, string mensagem);

    }
}
