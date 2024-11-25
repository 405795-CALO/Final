using AutoMapper;
using Final_Anashe.DTO.Queries;
using Final_Anashe.DTO.Responses;
using Final_Anashe.Modelos;

namespace Final_Anashe.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<Sucursal, ResponseSucursalDTO>();
            CreateMap<ResponseSucursalDTO, Sucursal>();

            CreateMap<Sucursal, PutSucursalDTO>();
            CreateMap<PutSucursalDTO, Sucursal>();

            CreateMap<Configuracion, ResponseConfiguracionDTO>();
            CreateMap<ResponseConfiguracionDTO, Configuracion>();

        }
    }
}
