using AutoMapper;
using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";
        private readonly IMapper mapper;
        public PeliculasController(ApplicationDBContext context, IAlmacenadorArchivos almacenadorArchivos, IMapper mapper)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<HomePageDTO>> Get()
        {
            var limite = 6;
            var peliculasEnCartelera = await context.Peliculas
                .Where(x => x.EnCartelera).Take(limite)
                .OrderByDescending(x => x.Lanzamiento)
                .ToListAsync();

            var fechaActual = DateTime.Now;
            var proximosEstrenos = await context.Peliculas
                .Where(x => x.Lanzamiento > fechaActual)
                .OrderBy(x => x.Lanzamiento).Take(limite)
                .ToListAsync();

            var response = new HomePageDTO()
            {
                PeliculasEnCartelera = peliculasEnCartelera,
                ProximosEstrenos = proximosEstrenos
            };

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisualizarPeliculaDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas.Where(x => x.Id == id)
                .Include(x => x.GenerosPelicula).ThenInclude(x => x.Genero)
                .Include(x => x.PeliculasActor).ThenInclude(x => x.Persona)
                .FirstOrDefaultAsync();

            if(pelicula == null) return NotFound();

            //todo: sistema de votacion
            var promedioVotos = 4;
            var votoUsuario = 5;

            pelicula.PeliculasActor = pelicula.PeliculasActor.OrderBy(x => x.Orden).ToList();

            var model = new VisualizarPeliculaDTO();
            model.Pelicula = pelicula;
            model.Generos = pelicula.GenerosPelicula.Select(x => x.Genero).ToList();
            model.Actores = pelicula.PeliculasActor.Select(x => new Persona
            {
                Nombre = x.Persona.Nombre,
                Foto = x.Persona.Foto,
                Personaje = x.Personaje,
                Id = x.PersonaId
            }).ToList();

            model.PromedioVotos = promedioVotos;
            model.VotoUsuario = votoUsuario;
            return model;
        }

        [HttpGet("actualizar/{id}")]
        public async Task<ActionResult<PeliculaActualizacionDTO>> PutGet(int id)
        {
            var peliculaActionResult = await Get(id);
            if (peliculaActionResult.Result is NotFoundResult) return NotFound();

            var peliculaVisualizarDTO = peliculaActionResult.Value;
            var generosSeleccionadosIds = peliculaVisualizarDTO.Generos.Select(x => x.Id).ToList();
            var generosNoSeleccionados = await context.Generos
                .Where(x => !generosSeleccionadosIds.Contains(x.Id))
                .ToListAsync();

            var model = new PeliculaActualizacionDTO();
            model.Pelicula = peliculaVisualizarDTO.Pelicula;
            model.Actores = peliculaVisualizarDTO.Actores;
            model.GenerosNoSeleccionados = generosNoSeleccionados;
            model.GenerosSeleccionados = peliculaVisualizarDTO.Generos;
            return model;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Pelicula pelicula)
        {
            //Subir el poster de una pelicula Azure o Local
            if (!string.IsNullOrEmpty(pelicula.Poster))
            {
                var fotoPersona = Convert.FromBase64String(pelicula.Poster);
                pelicula.Poster = await almacenadorArchivos.GuardarArchivo(fotoPersona, ".jpg", contenedor);
            }

            if(pelicula.PeliculasActor != null)
            {
                for (int i = 0; i < pelicula.PeliculasActor.Count; i++)
                {
                    pelicula.PeliculasActor[i].Orden = i + 1;
                }
            }

            context.Add(pelicula);
            await context.SaveChangesAsync();
            return pelicula.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Pelicula pelicula)
        {
            var peliculaDB = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == pelicula.Id);
            if (peliculaDB == null) return NotFound();

            peliculaDB = mapper.Map(pelicula, peliculaDB);

            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                var posterImagen = Convert.FromBase64String(pelicula.Poster);
                peliculaDB.Poster = await almacenadorArchivos.EditarArchivo(posterImagen, "jpg", contenedor, peliculaDB.Poster);
            }

            await context.Database.ExecuteSqlInterpolatedAsync($"delete from GenerosPeliculas WHERE PeliculaId = {pelicula.Id}; delete from PeliculasActores WHERE PeliculaId = {pelicula.Id}");

            if (pelicula.PeliculasActor != null)
            {
                for (int i = 0; i < pelicula.PeliculasActor.Count; i++)
                {
                    pelicula.PeliculasActor[i].Orden = i + 1;
                }
            }

            peliculaDB.PeliculasActor = pelicula.PeliculasActor;
            peliculaDB.GenerosPelicula = pelicula.GenerosPelicula;

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
