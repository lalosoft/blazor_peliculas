using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlazorPeliculas.Server.Helpers
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Editar un archivo en el Server
        /// </summary>
        /// <param name="contenido"></param>
        /// <param name="extension"></param>
        /// <param name="nubeContenedor"></param>
        /// <param name="ruta"></param>
        /// <returns></returns>
        public async Task<string> EditarArchivo(byte[] contenido, string extension, string nombreContenedor, string ruta)
        {
            await EliminarArchivo(ruta, nombreContenedor);
            return await GuardarArchivo(contenido, extension, nombreContenedor);
        }

        /// <summary>
        /// Eliminar un archivo de una carpeta del Server
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="nombreContenedor"></param>
        /// <returns></returns>
        public Task EliminarArchivo(string ruta, string nombreContenedor)
        {
            var filename = Path.GetFileName(ruta);
            string directorioArchivo = Path.Combine(env.WebRootPath, nombreContenedor, filename);
            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Guardar una imagen en una carpeta del Server
        /// </summary>
        /// <param name="contenido"></param>
        /// <param name="extension"></param>
        /// <param name="nombreContenedor"></param>
        /// <returns></returns>
        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor)
        {
            var filename = $"{Guid.NewGuid()}.{extension}";
            string folder = Path.Combine(env.WebRootPath, nombreContenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string rutaGuardado = Path.Combine(folder, filename);
            await File.WriteAllBytesAsync(rutaGuardado, contenido);

            var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var rutaParaDB = Path.Combine(urlActual, nombreContenedor, filename);
            return rutaParaDB;
        }
    }
}
