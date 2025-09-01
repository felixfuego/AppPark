using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using QRCodeData = Park.Comun.DTOs.QRCodeData;

namespace Park.Api.Services
{
    public class QRService : IQRService
    {
        private readonly string _secretKey = "ParkIndustrialSecretKey2024!@#";

        public async Task<string> GenerateQRCodeAsync(QRCodeData qrData)
        {
            return await Task.Run(() =>
            {
                var jsonData = JsonSerializer.Serialize(qrData);
                // En producción, aquí se generaría el QR real usando una librería como QRCoder
                // Por ahora retornamos el JSON codificado en Base64 como simulación
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonData));
            });
        }

        public async Task<string> GenerateQRCodeBase64Async(QRCodeData qrData)
        {
            return await GenerateQRCodeAsync(qrData);
        }

        public async Task<QRCodeData?> DecodeQRCodeAsync(string qrCodeData)
        {
            return await Task.Run(() =>
            {
                try
                {
                    // Decodificar el JSON del QR
                    var jsonBytes = Convert.FromBase64String(qrCodeData);
                    var jsonData = Encoding.UTF8.GetString(jsonBytes);
                    return JsonSerializer.Deserialize<QRCodeData>(jsonData);
                }
                catch
                {
                    return null;
                }
            });
        }

        public async Task<bool> ValidateQRCodeAsync(string qrCodeData, string securityHash)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var decodedData = DecodeQRCodeAsync(qrCodeData).Result;
                    if (decodedData == null) return false;

                    var expectedHash = GenerateSecurityHash(decodedData);
                    return expectedHash == securityHash;
                }
                catch
                {
                    return false;
                }
            });
        }

        public async Task<string> GenerateSecurityHashAsync(QRCodeData qrData)
        {
            return await Task.Run(() => GenerateSecurityHash(qrData));
        }

        public async Task<byte[]> GenerateQRCodeBytesAsync(QRCodeData qrData)
        {
            return await Task.Run(() =>
            {
                var jsonData = JsonSerializer.Serialize(qrData);
                return Encoding.UTF8.GetBytes(jsonData);
            });
        }

        private string GenerateSecurityHash(QRCodeData qrData)
        {
            var dataToHash = $"{qrData.VisitCode}|{qrData.VisitorName}|{qrData.CompanyName}|{qrData.ScheduledDate:yyyy-MM-dd HH:mm}|{qrData.GateId}|{_secretKey}";
            
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
