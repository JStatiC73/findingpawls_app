namespace FindingPawls.Models;

// Datos de autenticacion retornados por el API tras login o registro exitoso.
// IMPORTANTE: El API retorna los datos del usuario PLANOS (no anidados en un objeto Usuario).
// Nombres de propiedades deben coincidir EXACTAMENTE con el JSON del API (case-insensitive OK).
public class AuthResponse
{
    public Guid UsuarioID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    // El API retorna "AccessTokenExpiration", no "ExpiresAt".
    public DateTime AccessTokenExpiration { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
    public bool RequiereVerificacionCorreo { get; set; }
}

// Perfil del usuario autenticado almacenado en memoria durante la sesion.
// Se construye a partir de los campos planos de AuthResponse.
public class UsuarioPerfil
{
    public Guid UsuarioID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public float? Puntaje { get; set; }
    public DateTime? FechaRegistro { get; set; }
}

// Solicitud de login: correo y contrasena.
public class LoginRequest
{
    public string Correo { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
}

// Solicitud de registro de nuevo usuario.
public class RegisterRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public string ConfirmarContrasena { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}

// Solicitud de renovacion de token de acceso usando el refresh token.
public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
