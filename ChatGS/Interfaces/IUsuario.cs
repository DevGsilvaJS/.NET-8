﻿using ChatGS.DTO;
using ChatGS.Models.Users;
using ChatGS.ResponseModel;

namespace ChatGS.Interfaces
{
    public interface IUsuario
    {
        Task<ResponseModel<UsuarioModel>> GravarUsuario(UsuarioDTO usuario);
        Task<UsuarioModel> BuscaUsuarioPorID(int id);
        Task<List<UsuarioModel>> ListarUsuarios();
        Task<bool> DeletarUsuario(int ind);
        Task<UsuarioModel> AtualizarUsuario (UsuarioModel usuario);
        Task<bool> Autenticacao(string usuario, string senha);
    }
}
