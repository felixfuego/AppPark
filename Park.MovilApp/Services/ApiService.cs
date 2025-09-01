using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Park.MovilApp.Models;

namespace Park.MovilApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://fintotal.kattangroup.com/park/api"; // URL del servidor de producción
        private string? _authToken;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public void SetAuthToken(string token)
        {
            _authToken = token;
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public void ClearAuthToken()
        {
            _authToken = null;
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }

        public async Task<ApiResponse<UserInfo>> LoginAsync(string username, string password)
        {
            try
            {
                var loginData = new
                {
                    username = username,
                    password = password
                };

                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/auth/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<ApiResponse<UserInfo>>(responseContent);
                    if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
                    {
                        SetAuthToken(result.Token);
                    }
                    return result ?? new ApiResponse<UserInfo> { Success = false, Message = "Error al procesar la respuesta" };
                }
                else
                {
                    return new ApiResponse<UserInfo> { Success = false, Message = $"Error: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserInfo> { Success = false, Message = $"Error de conexión: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<UserInfo>> GetCurrentUserAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/auth/me");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var user = JsonConvert.DeserializeObject<UserInfo>(responseContent);
                    return new ApiResponse<UserInfo> { Success = true, User = user };
                }
                else
                {
                    return new ApiResponse<UserInfo> { Success = false, Message = $"Error: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserInfo> { Success = false, Message = $"Error de conexión: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<List<VisitInfo>>> GetVisitsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/visit");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var visits = JsonConvert.DeserializeObject<List<VisitInfo>>(responseContent);
                    return new ApiResponse<List<VisitInfo>> { Success = true, Data = visits };
                }
                else
                {
                    return new ApiResponse<List<VisitInfo>> { Success = false, Message = $"Error: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<VisitInfo>> { Success = false, Message = $"Error de conexión: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<List<VisitInfo>>> GetMyVisitsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/visit/my-visits");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var visits = JsonConvert.DeserializeObject<List<VisitInfo>>(responseContent);
                    return new ApiResponse<List<VisitInfo>> { Success = true, Data = visits };
                }
                else
                {
                    return new ApiResponse<List<VisitInfo>> { Success = false, Message = $"Error: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<VisitInfo>> { Success = false, Message = $"Error de conexión: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<List<VisitInfo>>> GetVisitsByGateAsync(int gateId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/visit/by-gate/{gateId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var visits = JsonConvert.DeserializeObject<List<VisitInfo>>(responseContent);
                    return new ApiResponse<List<VisitInfo>> { Success = true, Data = visits };
                }
                else
                {
                    return new ApiResponse<List<VisitInfo>> { Success = false, Message = $"Error: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<VisitInfo>> { Success = false, Message = $"Error de conexión: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<VisitInfo>> CheckInVisitAsync(CheckInRequest request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/visit/checkin", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var visit = JsonConvert.DeserializeObject<VisitInfo>(responseContent);
                    return new ApiResponse<VisitInfo> { Success = true, Data = visit };
                }
                else
                {
                    return new ApiResponse<VisitInfo> { Success = false, Message = $"Error: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<VisitInfo> { Success = false, Message = $"Error de conexión: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<VisitInfo>> CheckOutVisitAsync(CheckOutRequest request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/visit/checkout", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var visit = JsonConvert.DeserializeObject<VisitInfo>(responseContent);
                    return new ApiResponse<VisitInfo> { Success = true, Data = visit };
                }
                else
                {
                    return new ApiResponse<VisitInfo> { Success = false, Message = $"Error: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<VisitInfo> { Success = false, Message = $"Error de conexión: {ex.Message}" };
            }
        }

        public async Task<ApiResponse<VisitInfo>> GetVisitByCodeAsync(string visitCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/visit/code/{visitCode}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var visit = JsonConvert.DeserializeObject<VisitInfo>(responseContent);
                    return new ApiResponse<VisitInfo> { Success = true, Data = visit };
                }
                else
                {
                    return new ApiResponse<VisitInfo> { Success = false, Message = $"Error: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<VisitInfo> { Success = false, Message = $"Error de conexión: {ex.Message}" };
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(_authToken))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(_authToken), Encoding.UTF8, "application/json");
                    await _httpClient.PostAsync($"{_baseUrl}/auth/logout", content);
                }
                
                ClearAuthToken();
                return true;
            }
            catch
            {
                ClearAuthToken();
                return false;
            }
        }
    }
}
