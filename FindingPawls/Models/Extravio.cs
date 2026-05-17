namespace FindingPawls.Models;

// Reporte de mascota extraviada publicado por su dueno.
public class ReporteExtravio
{
    public Guid ReporteID { get; set; }
    public Guid MascotaID { get; set; }
    public string NombreMascota { get; set; } = string.Empty;
    public string? EspecieNombre { get; set; }
    public string? RazaNombre { get; set; }
    public string? SexoNombre { get; set; }
    public string? Color { get; set; }
    public string? NumeroMicrochip { get; set; }
    public string? Descripcion { get; set; }
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public double? DistanciaKm { get; set; }
    public DateTime? FechaExtravio { get; set; }
    public string Estado { get; set; } = "Activo";
    public string NombreDueno { get; set; } = string.Empty;
    public List<string> FotosMascotaUrls { get; set; } = new();
    public int CantidadAvistamientos { get; set; }
    public bool EsMiReporte { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public int? DiasDesaparecida { get; set; }
}

// DTO para crear un reporte de extravio.
public class CreateExtravioRequest
{
    public Guid MascotaID { get; set; }
    public DateTime? FechaExtravio { get; set; }
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public string? Descripcion { get; set; }
}

// Avistamiento de una mascota extraviada reportado por un usuario.
public class Avistamiento
{
    public Guid AvistamientoID { get; set; }
    public Guid ReporteID { get; set; }
    public string? FotoUrl { get; set; }
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public string? Descripcion { get; set; }
    public string NombreReportador { get; set; } = string.Empty;
    public DateTime? FechaAvistamiento { get; set; }
    public float? PorcentajeSimilitud { get; set; }
    public bool FueComparado { get; set; }
}

// DTO para reportar un avistamiento.
// La foto se envia como multipart/form-data desde MediaPicker.
public class CreateAvistamientoRequest
{
    public Guid ReporteExtravioID { get; set; }
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public string? Descripcion { get; set; }
}
