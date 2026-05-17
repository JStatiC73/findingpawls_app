using FindingPawls.Models;

namespace FindingPawls.Services.Auth;

// Servicio responsable de mantener el token JWT activo durante la sesion en curso.
// Los tokens se almacenan SOLO EN MEMORIA: al cerrar la app la sesion se pierde.
// Comportamiento correcto para una app de uso clinico/veterinario en dispositivos compartidos.
public class AuthTokenService
{
    // Almacenamiento en memoria. Se limpia automaticamente al terminar el proceso.
    private string? _accessToken;
    private string? _refreshToken;
    private DateTime _accessTokenExpiration = DateTime.MinValue;
    private Guid _usuarioId;
    private string _nombre = string.Empty;
    private string _correo = string.Empty;

    // --- Escritura ---

    // Guarda la sesion en memoria tras un login exitoso.
    // AuthResponse ahora tiene campos planos (sin objeto Usuario anidado).
    public Task GuardarSesionAsync(AuthResponse authResponse)
    {
        _accessToken = authResponse.AccessToken;
        _refreshToken = authResponse.RefreshToken;
        _accessTokenExpiration = authResponse.AccessTokenExpiration;
        _usuarioId = authResponse.UsuarioID;
        _nombre = authResponse.Nombre;
        _correo = authResponse.Correo;
        return Task.CompletedTask;
    }

    // Actualiza el access token tras un refresh exitoso.
    public Task ActualizarTokenAsync(string nuevoAccessToken, DateTime nuevaExpiracion)
    {
        _accessToken = nuevoAccessToken;
        _accessTokenExpiration = nuevaExpiracion;
        return Task.CompletedTask;
    }

    // Limpia todos los datos de sesion de la memoria.
    public void CerrarSesion()
    {
        _accessToken = null;
        _refreshToken = null;
        _accessTokenExpiration = DateTime.MinValue;
        _usuarioId = Guid.Empty;
        _nombre = string.Empty;
        _correo = string.Empty;
    }

    // --- Lectura ---

    public Task<string?> ObtenerAccessTokenAsync()
        => Task.FromResult(_accessToken);

    public Task<string?> ObtenerRefreshTokenAsync()
        => Task.FromResult(_refreshToken);

    // Verifica si hay token en memoria (usuario hizo login en esta sesion).
    public Task<bool> ExisteSesionAsync()
        => Task.FromResult(!string.IsNullOrEmpty(_accessToken));

    // Verifica si el access token es valido y no ha expirado (margen de 2 minutos).
    public Task<bool> TokenEsValidoAsync()
    {
        if (string.IsNullOrEmpty(_accessToken))
            return Task.FromResult(false);
        return Task.FromResult(_accessTokenExpiration > DateTime.UtcNow.AddMinutes(2));
    }

    // --- Datos del usuario en sesion ---

    public string ObtenerNombre() => _nombre;
    public string ObtenerCorreo() => _correo;
    public Guid ObtenerUsuarioId() => _usuarioId;

    // Retorna el perfil local del usuario autenticado.
    public Task<UsuarioPerfil?> ObtenerUsuarioLocalAsync()
    {
        if (string.IsNullOrEmpty(_accessToken)) return Task.FromResult<UsuarioPerfil?>(null);
        return Task.FromResult<UsuarioPerfil?>(new UsuarioPerfil
        {
            UsuarioID = _usuarioId,
            Nombre = _nombre,
            Correo = _correo
        });
    }
}
