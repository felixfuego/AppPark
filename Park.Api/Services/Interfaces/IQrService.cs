using System.Drawing;

namespace Park.Api.Services.Interfaces
{
    public interface IQrService
    {
        /// <summary>
        /// Genera un código QR como imagen PNG
        /// </summary>
        /// <param name="data">Datos a codificar en el QR</param>
        /// <param name="size">Tamaño de la imagen (píxeles)</param>
        /// <returns>Imagen PNG como array de bytes</returns>
        byte[] GenerateQrCode(string data, int size = 300);
        
        /// <summary>
        /// Genera un código QR para una visita específica
        /// </summary>
        /// <param name="visitaId">ID de la visita</param>
        /// <param name="size">Tamaño de la imagen (píxeles)</param>
        /// <returns>Imagen PNG como array de bytes</returns>
        byte[] GenerateVisitaQrCode(int visitaId, int size = 300);
    }
}
