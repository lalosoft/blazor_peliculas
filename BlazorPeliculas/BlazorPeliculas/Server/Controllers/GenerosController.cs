using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        public GenerosController(ApplicationDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Genero>>> Get() =>
            await context.Generos.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Genero>> Get(int id) =>
           await context.Generos.Where(x => x.Id == id).FirstOrDefaultAsync();

        [HttpPost]
        public async Task<ActionResult<int>> Post(Genero genero)
        {
            context.Add(genero);
            await context.SaveChangesAsync();
            return genero.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Genero genero)
        {
            context.Attach(genero).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
