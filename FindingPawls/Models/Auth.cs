namespace FindingPawls.Models;

// Datos de autenticacion retornados por el API tras login o registro exitoso.
public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UsuarioPerfil Usuario { get; set; } = new();
}

// Datos del usuario autenticado incluidos en la respuesta de auth.
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
