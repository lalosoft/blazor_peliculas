using AutoMapper;
using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Server.Helpers
{
    public class AutomapperPerfiles : Profile
    {
        public AutomapperPerfiles()
        {
            CreateMap<Persona, Persona>()
                .ForMember(x => x.Foto, option => option.Ignore());

            CreateMap<Pelicula, Pelicula>()
                .ForMember(x => x.Poster, option => option.Ignore());
        }
    }
}
