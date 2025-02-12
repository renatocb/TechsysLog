using AutoMapper;
using TechsysLog.Application.DTOs;
using TechsysLog.Domain.Entities;

namespace TechsysLog.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, RegistroUsuarioDto>().ReverseMap();            
            CreateMap<Pedido, PedidoDto>().ReverseMap();
            CreateMap<Pedido, ResponsePedidoDto>().ReverseMap();
            CreateMap<Pedido, ResponsePedidoDto>()
           .ForMember(dest => dest.Entrega, opt => opt.MapFrom(src => src.Entrega)); // Mapeia a propriedade Entrega
            CreateMap<Entrega, EntregaDto>().ReverseMap();
        }
    }
}
