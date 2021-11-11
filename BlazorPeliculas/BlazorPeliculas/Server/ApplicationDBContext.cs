using BlazorPeliculas.Shared.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculas.Server
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Creacion de llave primario compuesta en EF
            modelBuilder.Entity<GeneroPelicula>().HasKey(x => new { x.GeneroId, x.PeliculaId });
            modelBuilder.Entity<PeliculaActor>().HasKey(x => new { x.PelicualId, x.PersonaId });


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<GeneroPelicula> GenerosPeliculas { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<PeliculaActor> PeliculasActores { get; set; }
    }
}
