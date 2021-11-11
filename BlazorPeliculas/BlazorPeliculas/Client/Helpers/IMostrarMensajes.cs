using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Helpers
{
    public interface IMostrarMensajes
    {
        Task MostrarMensajeError(string msg);
    }
}
