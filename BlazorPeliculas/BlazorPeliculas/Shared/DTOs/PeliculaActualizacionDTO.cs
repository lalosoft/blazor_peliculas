using BlazorPeliculas.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTOs
{
    public class PeliculaActualizacionDTO
    {
        public Pelicula Pelicula { get; set; }
        public List<Persona> Actores { get; set; }
        public List<Genero> GenerosSeleccionados { get; set; }
        public List<Genero> GenerosNoSeleccionados { get; set; }
    }
}
