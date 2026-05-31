using API.Models.DTOs;
using API.Models.Entities;
using AutoMapper;

namespace API.Mappers
{
    public class ResenasProfile : Profile
    {
        public ResenasProfile()
        {
            CreateMap<CrearResenaDTO, Resena>();
            CreateMap<EditarResenaDTO, Resena>();
            CreateMap<Resena, ResenaDTO>()
                .ForMember(x => x.Imagen, y => y.MapFrom(z => $"/Uploads/{z.Id}.jpg"));
        }
    }
}
