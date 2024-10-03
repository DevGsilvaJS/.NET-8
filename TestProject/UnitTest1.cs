using ChatGS.Interfaces;
using ChatGS.Models.Transactions;
using ChatGS.Services.Transactions;
using Moq;
using AutoMapper; 
using Xunit; 

namespace TestProject
{
    public class UnitTest1
    {
        private readonly Mock<IRepositoryGeneric<object>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PlanoContasServices _service;

        // Construtor da classe de teste
        public UnitTest1()
        {
            _mockRepository = new Mock<IRepositoryGeneric<object>>();
            _mockMapper = new Mock<IMapper>(); 
            _service = new PlanoContasServices(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GravarPlanoContas_ShouldReturnError_WhenDescriptionIsLong()
        {
            // Arrange
            var planoContas = new PlanoContasModel
            {
                PCTID = 1,
                PCTTIPO = 'A',
                PCTDESCRICAO = new string('A', 300)
            };

            var result = await _service.GravarPlanoContas(planoContas);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal("Tamanho da descrição inválido.", result.Mensagem);
        }
    }
}
