using FindingPawls.Models;

namespace FindingPawls.Services.Auth;

// Servicio responsable de mantener el token JWT activo durante la sesion en curso.
//
// DECISION DE DISENO: Los tokens se almacenan SOLO EN MEMORIA (no en SecureStorage).
// Esto significa que al cerrar o matar la aplicacion, la sesion se pierde y el usuario
// debe autenticarse de nuevo al abrir la app. Este es el comportamiento correcto para
// una aplicacion de uso clinico/veterinario donde los dispositivos pueden ser compartidos.
//
// SecureStorage sigue disponible para una futura funcionalidad de "Recordarme"
// que se puede activar de forma opcional por el usuario.
public class AuthTokenService
{
    // Almacenamiento en memoria. Se limpia automaticamente al terminar el proceso.
    private string? _accessToken;
    private string? _refreshToken;
    private DateTime _expiresAt = DateTime.MinValue;
    private UsuarioPerfil? _usuarioPerfil;

    // --- Escritura ---

    // Guarda la sesion en memoria tras un login exitoso.
    // Solo se llama desde LoginViewModel (no desde RegisterViewModel).
    public Task GuardarSesionAsync(AuthResponse authResponse)
    {
        _accessToken = authResponse.AccessToken;
        _refreshToken = authResponse.RefreshToken;
        _expiresAt = authResponse.ExpiresAt;
        _usuarioPerfil = authResponse.Usuario;
        return Task.CompletedTask;
    }

    // Actualiza el access token tras un refresh exitoso.
    public Task ActualizarTokenAsync(string nuevoAccessToken, DateTime nuevaExpiracion)
    {
        _accessToken = nuevoAccessToken;
        _expiresAt = nuevaExpiracion;
        return Task.CompletedTask;
    }

    // Limpia todos los datos de sesion de la memoria.
    // Se llama al cerrar sesion explicitamente desde la app.
    public void CerrarSesion()
    {
        _accessToken = null;
        _refreshToken = null;
        _expiresAt = DateTime.MinValue;
        _usuarioPerfil = null;
    }

    // --- Lectura ---

    // Retorna el access token actual. Null si no hay sesion activa.
    public Task<string?> ObtenerAccessTokenAsync()
        => Task.FromResult(_accessToken);

    // Retorna el refresh token actual.
    public Task<string?> ObtenerRefreshTokenAsync()
        => Task.FromResult(_refreshToken);

    // Verifica si hay un token en memoria (= el usuario hizo login en esta sesion).
    // En un inicio limpio de la app, siempre retorna false porque la memoria esta limpia.
    public Task<bool> ExisteSesionAsync()
        => Task.FromResult(!string.IsNullOrEmpty(_accessToken));

    // Verifica si el access token es valido y no ha expirado (margen de 2 minutos).
    public Task<bool> TokenEsValidoAsync()
    {
        if (string.IsNullOrEmpty(_accessToken))
            return Task.FromResult(false);

        // Margen de 2 minutos para peticiones en vuelo.
        return Task.FromResult(_expiresAt > DateTime.UtcNow.AddMinutes(2));
    }

    // Retorna el perfil del usuario autenticado (datos guardados en memoria tras login).
    // Null si no hay sesion activa.
    public Task<UsuarioPerfil?> ObtenerUsuarioLocalAsync()
        => Task.FromResult(_usuarioPerfil);
}
