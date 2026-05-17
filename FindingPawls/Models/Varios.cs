namespace FindingPawls.Models;

// Item unificado del feed personalizado.
// Refleja EXACTAMENTE el FeedItemDto del API.
// - PublicacionID (no EntidadID)
// - FotoURL (mayuscula, no FotoUrl)
// - Tipo es string serializado desde el enum TipoPublicacion del API:
//   "Adopcion" | "Extravio" | "AnimalCallejero"
public class FeedItem
{
    public Guid PublicacionID { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? FotoURL { get; set; }
    public string? Especie { get; set; }
    public string? Raza { get; set; }
    public string? Color { get; set; }
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public double? DistanciaKm { get; set; }
    public string? Estado { get; set; }
    public DateTime? FechaPublicacion { get; set; }
    public string AutorNombre { get; set; } = string.Empty;

    // Helper para obtener el tipo de publicacion normalizado para navegacion.
    // "AnimalCallejero" del API se mapea a "Callejero" para las rutas de la app.
    public string TipoNormalizado => Tipo switch
    {
        "AnimalCallejero" => "Callejero",
        _ => Tipo
    };
}

// Reporte de animal callejero publicado por un usuario.
public class ReporteCallejero
{
    public Guid ReporteID { get; set; }
    public string? FotoUrl { get; set; }
    public List<string> FotosUrls { get; set; } = new();
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public double? DistanciaKm { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = "Activo";
    public string NombreReportador { get; set; } = string.Empty;
    public DateTime? FechaReporte { get; set; }
    public bool EsMiReporte { get; set; }
}

// DTO para crear un reporte de animal callejero.
public class CreateReporteCallejeroRequest
{
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}

// Notificacion del usuario.
public class Notificacion
{
    public Guid NotificacionID { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Cuerpo { get; set; } = string.Empty;
    public bool Leida { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public Guid? EntidadID { get; set; }
}

// Preferencias de notificaciones del usuario.
public class NotificacionPreferencias
{
    public bool Avistamientos { get; set; } = true;
    public bool SolicitudesAdopcion { get; set; } = true;
    public bool ExtraviosCercanos { get; set; } = true;
    public bool Calificaciones { get; set; } = true;
    public bool CallejerosCercanos { get; set; } = true;
}

// Estadisticas del perfil del usuario.
public class UsuarioEstadisticas
{
    public int MascotasRegistradas { get; set; }
    public int AdopcionesConcretadas { get; set; }
    public int AvistamientosReportados { get; set; }
    public int ReportesCallejeros { get; set; }
    public List<Insignia> Insignias { get; set; } = new();
    public List<CalificacionRecibida> Calificaciones { get; set; } = new();
}

// Insignia o badge obtenida por acciones de la comunidad.
public class Insignia
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? ImagenUrl { get; set; }
    public DateTime? FechaObtencion { get; set; }
}

// Calificacion recibida por otro usuario.
public class CalificacionRecibida
{
    public int Puntaje { get; set; }
    public string? Comentario { get; set; }
    public string NombreCalificador { get; set; } = string.Empty;
    public DateTime? FechaCalificacion { get; set; }
}
