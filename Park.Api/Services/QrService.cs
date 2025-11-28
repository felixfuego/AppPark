using Park.Api.Services.Interfaces;
using System.Text.Json;

namespace Park.Api.Services
{
    public class QrService : IQrService
    {
        public byte[] GenerateQrCode(string data, int size = 300)
        {
            // TODO: Implementar generación de QR cuando se resuelva el problema con QRCoder
            // Por ahora retornamos un array vacío
            return new byte[0];
        }

        public byte[] GenerateVisitaQrCode(int visitaId, int size = 300)
        {
            // TODO: Implementar generación de QR cuando se resuelva el problema con QRCoder
            // Por ahora retornamos un array vacío
            return new byte[0];
        }
    }
}