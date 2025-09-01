using Newtonsoft.Json;
using Park.Maui.Models;
using Park.Maui.Utils;

namespace Park.Maui.Services
{
    public class VisitService : IVisitService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly string _baseUrl;

        public VisitService(IAuthService authService)
        {
            _authService = authService;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(Constants.RequestTimeout);
            _baseUrl = Constants.BaseApiUrl;
        }

        private async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            var token = await _authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return _httpClient;
        }

        public async Task<List<Visit>?> GetVisitsAsync()
        {
            try
            {
                var client = await GetAuthenticatedClientAsync();
                var response = await client.GetAsync($"{_baseUrl}{Constants.VisitsEndpoint}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Visit>>(content);
                }
                
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo visitas: {ex.Message}");
                return null;
            }
        }

        public async Task<Visit?> GetVisitByIdAsync(int visitId)
        {
            try
            {
                var client = await GetAuthenticatedClientAsync();
                var response = await client.GetAsync($"{_baseUrl}{Constants.VisitsEndpoint}/{visitId}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Visit>(content);
                }
                
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo visita {visitId}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CheckInVisitAsync(int visitId, int gateId)
        {
            try
            {
                var client = await GetAuthenticatedClientAsync();
                var checkInRequest = new { VisitId = visitId, GateId = gateId };
                var json = JsonConvert.SerializeObject(checkInRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync($"{_baseUrl}{Constants.CheckInEndpoint}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en check-in: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CheckOutVisitAsync(int visitId, int gateId)
        {
            try
            {
                var client = await GetAuthenticatedClientAsync();
                var checkOutRequest = new { VisitId = visitId, GateId = gateId };
                var json = JsonConvert.SerializeObject(checkOutRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync($"{_baseUrl}{Constants.CheckOutEndpoint}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en check-out: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Visit>?> GetVisitsByStatusAsync(string status)
        {
            try
            {
                var allVisits = await GetVisitsAsync();
                return allVisits?.Where(v => v.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error filtrando visitas por estado: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Visit>?> GetVisitsByCompanyAsync(int companyId)
        {
            try
            {
                var client = await GetAuthenticatedClientAsync();
                var response = await client.GetAsync($"{_baseUrl}{Constants.VisitsEndpoint}/by-company/{companyId}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Visit>>(content);
                }
                
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo visitas por empresa: {ex.Message}");
                return null;
            }
        }
    }
}
