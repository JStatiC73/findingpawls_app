namespace FindingPawls.Models;

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

// Item del feed combinado: puede ser extravio, adopcion o callejero.
public class FeedItem
{
    public Guid EntidadID { get; set; }
    public string Tipo { get; set; } = string.Empty; // "Extravio" | "Adopcion" | "Callejero"
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? FotoUrl { get; set; }
    public double? DistanciaKm { get; set; }
    public DateTime? FechaPublicacion { get; set; }
    public string Estado { get; set; } = string.Empty;
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
