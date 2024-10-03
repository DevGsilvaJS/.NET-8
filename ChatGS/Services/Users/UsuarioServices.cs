using AutoMapper;
using ChatGS.DTO;
using ChatGS.Interfaces;
using ChatGS.Models.Users;
using ChatGS.Querys;
using ChatGS.ResponseModel;
using System.Text;
using System.Transactions;


namespace ChatGS.Services.Users
{
    public class UsuarioService
    {
        private readonly IRepositoryGeneric<object> _repository;
        private readonly IMapper _mapper;

        public UsuarioService(IRepositoryGeneric<object> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ResponseModel<UsuarioModel>> GravarUsuario(UsuarioDTO usuarioDTO)
        {
            var response = new ResponseModel<UsuarioModel>();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    var usuario = _mapper.Map<UsuarioModel>(usuarioDTO);

                    var pessoaResponse = await _repository.Insert(usuario.Pessoa, "TB_PES_PESSOA");

                    usuario.PESID = pessoaResponse.PESID;

                    var usuarioResponse = await _repository.Insert(usuario, "TB_USU_USUARIO");


                    scope.Complete();

                    response.Data = usuarioResponse;
                    response.Mensagem = "Usuário inserido com sucesso.";
                    response.Success = true;
                }
                catch (Exception ex)
                {

                    response.Data = false;
                    response.Mensagem = $"Erro ao cadastrar usuário: {ex.Message}";
                    return response;
                }
            }

            return response;
        }
        public async Task<UsuarioModel> GetById(int id)
        {
            UsuarioQuery UsuarioQuery = new UsuarioQuery();


            var parameters = new { PESID = id };

            using (var reader = await _repository.GetById(UsuarioQuery.GetUserById(), parameters))
            {
                UsuarioModel usuario = null;

                if (reader.Read())
                {

                    usuario = new UsuarioModel
                    {
                        USUID = Convert.ToInt32(reader["USUID"]),
                        PESID = Convert.ToInt32(reader["PESID"]),
                        USULOGIN = reader["USULOGIN"].ToString(),
                        USUSENHA = reader["USUSENHA"].ToString(),

                        Pessoa = new PessoaModel
                        {
                            PESID = Convert.ToInt32(reader["PESID"]),
                            PESNOME = reader["PESNOME"].ToString(),
                            PESSOBRENOME = reader["PESSOBRENOME"].ToString(),
                            PESDATACADASTRO = Convert.ToDateTime(reader["PESDATACADASTRO"])
                        }
                    };
                }

                return usuario;
            }
        }
        public async Task<bool> DeleteUsuario(int id)
        {
            // Utiliza o TransactionScope com suporte a operações assíncronas
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string deletePessoa = "DELETE FROM TB_PES_PESSOA WHERE PESID = @PESID";
                    string deleteUsuario = "DELETE FROM TB_USU_USUARIO WHERE PESID = @PESID";

                    var excluirUsuario = await _repository.Delete(deleteUsuario, new { PESID = id });
                    var excluirPessoa = await _repository.Delete(deletePessoa, new { PESID = id });



                    if (excluirPessoa && excluirUsuario)
                    {
                        scope.Complete(); // Completa a transação
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao excluir o usuário e a pessoa.", ex);
                }
            }
        }
        public async Task<List<UsuarioModel>> ListAll()
        {
            UsuarioQuery query = new UsuarioQuery();


            var result = await _repository.ListAll(query.ListAllUsers());

            var usuarios = new List<UsuarioModel>();

            foreach (var row in result)
            {

                var rowDict = (IDictionary<string, object>)row;

                var usuario = new UsuarioModel
                {
                    USUID = Convert.ToInt32(rowDict["USUID"]),
                    USULOGIN = rowDict["USULOGIN"].ToString(),
                    USUSENHA = rowDict["USUSENHA"].ToString(),
                    Pessoa = new PessoaModel
                    {
                        PESID = Convert.ToInt32(rowDict["PESID"]),
                        PESNOME = rowDict["PESNOME"].ToString(),
                        PESSOBRENOME = rowDict["PESSOBRENOME"].ToString()
                    }
                };

                usuarios.Add(usuario);
            }

            return usuarios;
        }



    }
}
