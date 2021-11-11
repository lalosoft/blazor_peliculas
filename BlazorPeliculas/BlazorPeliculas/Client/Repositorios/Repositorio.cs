using BlazorPeliculas.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Repositorios
{
    public class Repositorio : IRepositorio
    {
        private readonly HttpClient httpClient;
        public Repositorio(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, enviarContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }
        public List<Pelicula> ObtenerPeliculas()
        {
            return new List<Pelicula>() 
            {
                new Pelicula() { Titulo = "Gangster Americano", Lanzamiento = DateTime.Now,
                                  Poster = "https://images-na.ssl-images-amazon.com/images/S/pv-target-images/51a869a2be201e619c77534f756e6242c094f73ce74a642ccf70eceb475857d4._RI_V_TTW_.jpg"},
                new Pelicula() { Titulo = "60 Segundos", Lanzamiento = DateTime.Now,
                                 Poster = "https://www.themoviedb.org/t/p/w500/h3kpgUWhCaiDCHjGLq7vexl27fE.jpg"},
                new Pelicula() { Titulo = "El transportador", Lanzamiento = DateTime.Now,
                                 Poster = "https://www.themoviedb.org/t/p/original/A6jWpnsmiFJR9idlCOUCeP3m03I.jpg"}
            };
        }
    }
}
