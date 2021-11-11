using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlazorPeliculas.Server.Helpers
{
    public class AlmacenadorArchivosAS : IAlmacenadorArchivos
    {
        private string connectionString;
        public AlmacenadorArchivosAS(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }

        /// <summary>
        /// Editar un archivo en Azure Storage
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
        /// Borrar un archivo de Azure Storage
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="nombreContenedor"></param>
        /// <returns></returns>
        public async Task EliminarArchivo(string ruta, string nombreContenedor)
        {
            if (string.IsNullOrEmpty(ruta)) return;

            var cliente = new BlobContainerClient(connectionString, nombreContenedor);
            await cliente.CreateIfNotExistsAsync();
            var nombreArchivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(nombreArchivo);
            await blob.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Subir una imagen a Azure Storage
        /// </summary>
        /// <param name="contenido"></param>
        /// <param name="extension"></param>
        /// <param name="nombreContenedor"></param>
        /// <returns></returns>
        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor)
        {
            var cliente = new BlobContainerClient(connectionString, nombreContenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var archivoNombre = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(archivoNombre);
            using(var ms = new MemoryStream(contenido))
            {
                await blob.UploadAsync(ms);
            }
            return blob.Uri.ToString();
        }
    }
}
