using AutoMapper;
using ChatGS.Interfaces;
using ChatGS.Models.Transactions;
using ChatGS.Models.Users;
using ChatGS.ResponseModel;
using System.Transactions;

namespace ChatGS.Services.Transactions
{
    public class PlanoContasServices
    {
        private readonly IRepositoryGeneric<object> _repository;
        private readonly IMapper _mapper;

        public PlanoContasServices(IRepositoryGeneric<object> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<ResponseModel<PlanoContasModel>> GravarPlanoContas(PlanoContasModel planoContas)
        {
            var response = new ResponseModel<PlanoContasModel>();


            if (planoContas.PCTDESCRICAO.Length > 50)
            {
                response.Data = planoContas;
                response.Mensagem = "Tamanho da descrição inválido.";
                response.Success = false;

                return response;
            }


            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                try
                {
                    var planoResponse = await _repository.Insert(planoContas, "TB_PCT_PLANOCONTAS");

                    scope.Complete();

                    response.Data = planoContas;
                    response.Mensagem = "Plano de contas cadastrado com sucesso.";
                    response.Success = true;

 
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            return response;

        }

    }
}
