using AutoMapper;
using ChatGS.DTO;
using ChatGS.Interfaces;
using ChatGS.Models;
using ChatGS.ResponseModel;
using ChatGS.Services;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Transactions;
using static ChatGS.Services.BaseLista;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChatGS.Servicos
{
    public class UsuarioService : IUsuario
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public UsuarioService(IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DataBase");
            _mapper = mapper;
        }
        public Task<UsuarioModel> AtualizarUsuario(UsuarioModel usuario)
        {
            throw new NotImplementedException();
        }
        public Task<UsuarioModel> BuscaUsuarioPorID(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeletarUsuario(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Passo 1: Recuperar GUSID e PESID relacionados ao usuário
                        var sqlSelect = @"
                    SELECT GUSID, PESID 
                    FROM TB_USU_USUARIO 
                    WHERE USUID = @Id";

                        var usuario = await connection.QuerySingleOrDefaultAsync<dynamic>(sqlSelect, new { Id = id }, transaction: transaction);

                        if (usuario == null)
                        {
                            return false;
                        }

                        var gusId = usuario.GUSID;
                        var pesId = usuario.PESID;

                        // Passo 2: Excluir registros das tabelas relacionadas
                        var sqlDeleteUsuario = "DELETE FROM TB_USU_USUARIO WHERE USUID = @Id";
                        var sqlDeleteGrupo = "DELETE FROM TB_GUS_GRUPOUSUARIO WHERE GUSID = @GusId";
                        var sqlDeletePessoa = "DELETE FROM TB_PES_PESSOA WHERE PESID = @PesId";

                        await connection.ExecuteAsync(sqlDeleteUsuario, new { Id = id }, transaction: transaction);

                        if (gusId != null)
                        {
                            await connection.ExecuteAsync(sqlDeleteGrupo, new { GusId = gusId }, transaction: transaction);
                        }
                        if (pesId != null)
                        {
                            await connection.ExecuteAsync(sqlDeletePessoa, new { PesId = pesId }, transaction: transaction);
                        }

                        transaction.Commit();

                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task<ResponseModel<UsuarioModel>> GravarUsuario(UsuarioDTO usuarioDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var generator = new RetornaQueryGravacao();

                        var usuario = _mapper.Map<UsuarioModel>(usuarioDTO);

                        ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();

                        var (sqlPessoa, parametersPessoa) = generator.GenerateInsertCommand(usuario.Pessoa, "TB_PES_PESSOA");
                        var pesid = await connection.QuerySingleAsync<int>(sqlPessoa, parametersPessoa, transaction: transaction);

                        usuario.PESID = pesid;



                        if (usuarioDTO.GrupoUsuarios.Id == 0) {

                            return new ResponseModel<UsuarioModel>
                            {
                                Dados = usuario,
                                Mensagem = "Grupo de usuário não preenchido!"
                            };

                        }

                        usuario.GUSID = usuarioDTO.GrupoUsuarios.Id;

                        var (sqlUsuario, parametersUsuario) = generator.GenerateInsertCommand(usuario, "TB_USU_USUARIO");

                 
                        var usuid = await connection.QuerySingleAsync<int>(sqlUsuario, parametersUsuario, transaction: transaction);

                        transaction.Commit();

                        return new ResponseModel<UsuarioModel>
                        {
                            Dados = usuario,
                            Mensagem = "Cliente cadastrado com sucesso!"
                        };
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw; 
                    }
                }
            }
        }
        public async Task<List<UsuarioModel>> ListarUsuarios()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"
            SELECT 
                USU.USUID,
                USU.PESID,
                USU.USULOGIN,
                USU.USUSENHA,
                PES.PESNOME,
                PES.PESSOBRENOME,
                GUS.GUSID,
                GUS.GUSDESCRICAO
            FROM TB_USU_USUARIO USU
            JOIN TB_PES_PESSOA PES ON PES.PESID = USU.PESID
            JOIN TB_GUS_GRUPOUSUARIO GUS ON GUS.GUSID = USU.GUSID";

                var resultado = await connection.QueryAsync<dynamic>(sql);

                var usuarios = resultado.Select(item => new UsuarioModel
                {
                    USUID = item.USUID,
                    USULOGIN = item.USULOGIN,
                    USUSENHA = item.USUSENHA,
                    Pessoa = new PessoaModel
                    {
                        PESID = item.PESID,
                        PESNOME = item.PESNOME,
                        PESSOBRENOME = item.PESSOBRENOME
                    },
                    GrupoUsuarios = new GrupoUsuarioModel
                    {
                        GUSID = item.GUSID,
                        GUSDESCRICAO = item.GUSDESCRICAO
                    }
                }).ToList();

                return usuarios;
            }
        }


    }
}
