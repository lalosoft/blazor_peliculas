﻿using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "personas";
        private readonly IMapper mapper;

        public PersonasController(ApplicationDBContext context, IAlmacenadorArchivos almacenadorArchivos, IMapper mapper)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Persona>>> Get()
        {
            return await context.Personas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> Get(int id)
        {
            var persona = await context.Personas.FirstOrDefaultAsync(x => x.Id == id);
            if(persona == null) return NotFound();
            return persona;
        }

        [HttpGet("buscar/{textoBusqueda}")]
        public async Task<ActionResult<List<Persona>>> Get(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda)) return new List<Persona>();
            textoBusqueda = textoBusqueda.ToLower();
            return await context.Personas.Where(x => x.Nombre.ToLower().Contains(textoBusqueda)).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Persona persona)
        {
            //Subir la foto de una persona a Azure o Local
            if (!string.IsNullOrEmpty(persona.Foto))
            {
                var fotoPersona = Convert.FromBase64String(persona.Foto);
                persona.Foto = await almacenadorArchivos.GuardarArchivo(fotoPersona, ".jpg", contenedor);
            }

            context.Add(persona);
            await context.SaveChangesAsync();
            return persona.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Persona persona)
        {
            var personaDB = await context.Personas.FirstOrDefaultAsync(x => x.Id == persona.Id);
            if (personaDB == null) return NotFound();

            personaDB = mapper.Map(persona, personaDB);
            if (!string.IsNullOrWhiteSpace(persona.Foto))
            {
                var fotoImg = Convert.FromBase64String(persona.Foto);
                personaDB.Foto = await almacenadorArchivos.EditarArchivo(fotoImg, "jpg", contenedor, personaDB.Foto);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
