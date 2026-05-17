using FindingPawls.Models;
using FindingPawls.Services.Auth;
using System.Net.Http.Json;
using System.Text.Json;

namespace FindingPawls.Services.Api;

// Cliente HTTP base que centraliza configuracion comun para todos los servicios de API.
// Responsabilidades:
//   - Adjuntar el Bearer token JWT en cada peticion.
//   - Renovar el token automaticamente si esta a punto de expirar.
//   - Redirigir al login si el refresh token tambien expiro.
//   - Deserializar respuestas usando la convencion de nombres del API.
public class BaseApiService
{
    protected readonly HttpClient _httpClient;
    protected readonly AuthTokenService _tokenService;

    // Opciones de serializacion JSON consistentes con el API (camelCase por defecto).
    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public BaseApiService(HttpClient httpClient, AuthTokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }

    // Prepara la peticion adjuntando el token de autorizacion.
    // Si el token esta por expirar, intenta renovarlo primero.
    // Si no se puede renovar, limpia la sesion y lanza una excepcion
    // para que la capa de navegacion redirija al login.
    protected async Task PrepararAutorizacionAsync()
    {
        bool tokenValido = await _tokenService.TokenEsValidoAsync();

        if (!tokenValido)
        {
            string? refreshToken = await _tokenService.ObtenerRefreshTokenAsync();

            if (string.IsNullOrEmpty(refreshToken))
            {
                _tokenService.CerrarSesion();
                throw new UnauthorizedAccessException("La sesion ha expirado. Por favor inicia sesion nuevamente.");
            }

            await RenovarTokenAsync(refreshToken);
        }

        string? accessToken = await _tokenService.ObtenerAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
    }

    // Intenta renovar el access token usando el refresh token.
    // Si el servidor rechaza el refresh, cierra la sesion.
    private async Task RenovarTokenAsync(string refreshToken)
    {
        try
        {
            RefreshTokenRequest request = new() { RefreshToken = refreshToken };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/refresh-token", request, JsonOptions);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<AuthResponse>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>(JsonOptions);

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    await _tokenService.GuardarSesionAsync(apiResponse.Data);
                    return;
                }
            }

            // Si el refresh fallo, la sesion ya no es recuperable.
            _tokenService.CerrarSesion();
            throw new UnauthorizedAccessException("La sesion ha expirado. Por favor inicia sesion nuevamente.");
        }
        catch (UnauthorizedAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Error de red u otro inesperado durante el refresh.
            throw new Exception($"No se pudo renovar la sesion: {ex.Message}", ex);
        }
    }

    // Extrae el mensaje de error de una respuesta HTTP fallida.
    // Intenta deserializar el cuerpo como ErrorResponse del API.
    protected async Task<string> ObtenerMensajeErrorAsync(HttpResponseMessage response)
    {
        try
        {
            ErrorResponse? errorBody =
                await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions);

            if (errorBody != null && !string.IsNullOrEmpty(errorBody.Message))
                return errorBody.Message;

            if (errorBody?.Errors?.Count > 0)
                return string.Join(", ", errorBody.Errors);
        }
        catch
        {
            // Si no se puede deserializar, usa el status code como descripcion.
        }

        return $"Error {(int)response.StatusCode}: {response.ReasonPhrase}";
    }
}
