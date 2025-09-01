using Microsoft.Extensions.Logging;
using Park.Web.Models;
using System.Net.Http.Json;

namespace Park.Web.Services;

public class VisitService : BaseApiService<Visit, CreateVisit, UpdateVisit>, IVisitService
{
    public VisitService(HttpClientService httpClientService, ILogger<VisitService> logger) 
        : base(httpClientService, logger, "api/visit")
    {
    }

    public async Task<Visit?> GetByCodeAsync(string visitCode)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/code/{visitCode}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Visit>();
            }
            
            _logger.LogWarning("Error al obtener visita por código {Code}: {StatusCode}", visitCode, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener visita por código {Code}", visitCode);
            return null;
        }
    }

    public async Task<Visit?> GetByVisitCodeAsync(string visitCode)
    {
        return await GetByCodeAsync(visitCode);
    }

    public async Task<IEnumerable<Visit>?> GetByVisitorAsync(int visitorId)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/visitor/{visitorId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Visit>>();
            }
            
            _logger.LogWarning("Error al obtener visitas del visitante {VisitorId}: {StatusCode}", visitorId, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener visitas del visitante {VisitorId}", visitorId);
            return null;
        }
    }

    public async Task<IEnumerable<Visit>?> GetByCompanyAsync(int companyId)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/company/{companyId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Visit>>();
            }
            
            _logger.LogWarning("Error al obtener visitas de la empresa {CompanyId}: {StatusCode}", companyId, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener visitas de la empresa {CompanyId}", companyId);
            return null;
        }
    }

    public async Task<Visit?> CheckInAsync(VisitCheckIn checkIn)
    {
        try
        {
            var jsonContent = JsonContent.Create(checkIn);
            var response = await _httpClientService.PostAsync($"{_baseUrl}/checkin", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Visit>();
            }
            
            _logger.LogWarning("Error al hacer check-in de la visita: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al hacer check-in de la visita");
            return null;
        }
    }

    public async Task<Visit?> CheckOutAsync(VisitCheckOut checkOut)
    {
        try
        {
            var jsonContent = JsonContent.Create(checkOut);
            var response = await _httpClientService.PostAsync($"{_baseUrl}/checkout", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Visit>();
            }
            
            _logger.LogWarning("Error al hacer check-out de la visita: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al hacer check-out de la visita");
            return null;
        }
    }

    public async Task<Visit?> UpdateStatusAsync(int id, VisitStatusUpdate statusUpdate)
    {
        try
        {
            var jsonContent = JsonContent.Create(statusUpdate);
            var response = await _httpClientService.PutAsync($"{_baseUrl}/{id}/status", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Visit>();
            }
            
            _logger.LogWarning("Error al actualizar estado de visita {Id}: {StatusCode}", id, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estado de visita {Id}", id);
            return null;
        }
    }

    public async Task<IEnumerable<Visit>?> GetByGateAsync(int gateId)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/gate/{gateId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Visit>>();
            }
            
            _logger.LogWarning("Error al obtener visitas por puerta {GateId}: {StatusCode}", gateId, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener visitas por puerta {GateId}", gateId);
            return null;
        }
    }

    public async Task<IEnumerable<Visit>?> GetByStatusAsync(string status)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/status/{status}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Visit>>();
            }
            
            _logger.LogWarning("Error al obtener visitas por estado {Status}: {StatusCode}", status, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener visitas por estado {Status}", status);
            return null;
        }
    }

    public async Task<IEnumerable<Visit>?> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/daterange?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Visit>>();
            }
            
            _logger.LogWarning("Error al obtener visitas por rango de fechas {StartDate}-{EndDate}: {StatusCode}", startDate, endDate, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener visitas por rango de fechas {StartDate}-{EndDate}", startDate, endDate);
            return null;
        }
    }

    public async Task<string?> GenerateQRCodeAsync(int visitId)
    {
        try
        {
            var response = await _httpClientService.GetAsync($"{_baseUrl}/{visitId}/qrcode");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            
            _logger.LogWarning("Error al generar QR para visita {VisitId}: {StatusCode}", visitId, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar QR para visita {VisitId}", visitId);
            return null;
        }
    }

    public async Task<bool> ValidateQRCodeAsync(string qrCodeData)
    {
        try
        {
            var jsonContent = JsonContent.Create(new { QRCode = qrCodeData });
            var response = await _httpClientService.PostAsync($"{_baseUrl}/validate-qr", jsonContent);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar QR code");
            return false;
        }
    }

    public async Task<Visit?> GetByQRCodeAsync(string qrCodeData)
    {
        try
        {
            var jsonContent = JsonContent.Create(new { QRCode = qrCodeData });
            var response = await _httpClientService.PostAsync($"{_baseUrl}/qrcode", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Visit>();
            }
            
            _logger.LogWarning("Error al obtener visita por QR: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener visita por QR");
            return null;
        }
    }
}
