using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.Entidades
{
    public class PeliculaActor
    {
        public int PersonaId { get; set; }
        public int PelicualId { get; set; }
        public Persona Persona { get; set; }
        public Pelicula Pelicula { get; set; }
        public string Personaje { get; set; }
        public int Orden { get; set; }
        public List<PeliculaActor> PeliculasActor { get; set; }
    }
}
