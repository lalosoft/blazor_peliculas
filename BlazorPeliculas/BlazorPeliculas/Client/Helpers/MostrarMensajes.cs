using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Helpers
{
    public class MostrarMensajes : IMostrarMensajes
    {
        public async Task MostrarMensajeError(string msg)
        {
            await Task.FromResult(0);
        }
    }
}
