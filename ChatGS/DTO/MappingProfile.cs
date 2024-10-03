using AutoMapper;
using ChatGS.DTO;
using ChatGS.Models.Users;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<PessoaModel, PessoaDTO>();
        CreateMap<PessoaDTO, PessoaModel>();





        CreateMap<UsuarioDTO, UsuarioModel>()
            .ForMember(dest => dest.USULOGIN, opt => opt.MapFrom(src => src.NomeUsuario))
            .ForMember(dest => dest.USUSENHA, opt => opt.MapFrom(src => src.Senha))
            .ForMember(dest => dest.Pessoa, opt => opt.MapFrom(src => src.Pessoa));


        CreateMap<PessoaDTO, PessoaModel>()
           .ForMember(dest => dest.PESNOME, opt => opt.MapFrom(src => src.Nome))
           .ForMember(dest => dest.PESSOBRENOME, opt => opt.MapFrom(src => src.SobreNome))
           .ForMember(dest => dest.PESDATACADASTRO, opt => opt.MapFrom(src => DateTime.Now));
    }
}
