using FindingPawls.Models;
using FindingPawls.Services.Auth;
using System.Net.Http.Json;

namespace FindingPawls.Services.Api;

// Servicio de autenticacion: login, registro, logout y refresh token.
// No requiere token previo para login y registro, por eso no hereda de BaseApiService
// para esas operaciones, pero si para logout (requiere sesion activa).
public class AuthApiService : BaseApiService
{
    public AuthApiService(HttpClient httpClient, AuthTokenService tokenService)
        : base(httpClient, tokenService)
    {
    }

    // Inicia sesion con correo y contrasena.
    // Retorna (exito, respuesta, mensajeError).
    public async Task<(bool Exito, AuthResponse? Respuesta, string? Error)> LoginAsync(LoginRequest request)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/login", request, JsonOptions);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<AuthResponse>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>(JsonOptions);

                if (apiResponse?.Success == true && apiResponse.Data != null)
                    return (true, apiResponse.Data, null);

                return (false, null, apiResponse?.Message ?? "Login fallido.");
            }

            string error = await ObtenerMensajeErrorAsync(response);
            return (false, null, error);
        }
        catch (Exception ex)
        {
            return (false, null, $"No se pudo conectar al servidor: {ex.Message}");
        }
    }

    // Registra un nuevo usuario en la aplicacion.
    // Retorna (exito, respuesta, mensajeError).
    public async Task<(bool Exito, AuthResponse? Respuesta, string? Error)> RegistrarAsync(RegisterRequest request)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/register", request, JsonOptions);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<AuthResponse>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>(JsonOptions);

                if (apiResponse?.Success == true && apiResponse.Data != null)
                    return (true, apiResponse.Data, null);

                return (false, null, apiResponse?.Message ?? "Registro fallido.");
            }

            string error = await ObtenerMensajeErrorAsync(response);
            return (false, null, error);
        }
        catch (Exception ex)
        {
            return (false, null, $"No se pudo conectar al servidor: {ex.Message}");
        }
    }

    // Cierra la sesion del usuario actual en el servidor.
    // Elimina el refresh token para invalidar la sesion en el API.
    public async Task<bool> LogoutAsync()
    {
        try
        {
            string? refreshToken = await _tokenService.ObtenerRefreshTokenAsync();
            if (string.IsNullOrEmpty(refreshToken))
                return true;

            await PrepararAutorizacionAsync();

            RefreshTokenRequest logoutRequest = new() { RefreshToken = refreshToken };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("auth/logout", logoutRequest, JsonOptions);

            // Independientemente de la respuesta del servidor, limpiamos la sesion local.
            _tokenService.CerrarSesion();
            return response.IsSuccessStatusCode;
        }
        catch
        {
            // Si falla la llamada al servidor, igual cerramos la sesion local.
            _tokenService.CerrarSesion();
            return true;
        }
    }
}
