using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IQRService
    {
        Task<string> GenerateQRCodeAsync(QRCodeData qrData);
        Task<string> GenerateQRCodeBase64Async(QRCodeData qrData);
        Task<QRCodeData?> DecodeQRCodeAsync(string qrCodeData);
        Task<bool> ValidateQRCodeAsync(string qrCodeData, string securityHash);
        Task<string> GenerateSecurityHashAsync(QRCodeData qrData);
        Task<byte[]> GenerateQRCodeBytesAsync(QRCodeData qrData);
    }
}
