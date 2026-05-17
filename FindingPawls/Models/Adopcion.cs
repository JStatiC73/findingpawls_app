namespace FindingPawls.Models;

// Publicacion de adopcion de una mascota.
public class Adopcion
{
    public Guid AdopcionID { get; set; }
    public Guid MascotaID { get; set; }
    public string NombreMascota { get; set; } = string.Empty;
    public string? EspecieNombre { get; set; }
    public string? RazaNombre { get; set; }
    public string? SexoNombre { get; set; }
    public int? EdadMeses { get; set; }
    public bool? Esterilizado { get; set; }
    public string? Descripcion { get; set; }
    public string? InformacionContacto { get; set; }
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public double? DistanciaKm { get; set; }
    public string Estado { get; set; } = "Disponible";
    public string NombrePublicador { get; set; } = string.Empty;
    public float? PuntajePublicador { get; set; }
    public List<string> FotosUrls { get; set; } = new();
    public DateTime? FechaPublicacion { get; set; }
    public bool EsMiPublicacion { get; set; }
}

// Filtros de busqueda para adopciones cercanas.
public class AdopcionBusquedaParams
{
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public double RadioKm { get; set; } = 10;
    public int? EspecieID { get; set; }
    public int? SexoID { get; set; }
    public int Pagina { get; set; } = 1;
    public int TamanoPagina { get; set; } = 20;
}

// DTO para publicar una mascota en adopcion.
public class CreateAdopcionRequest
{
    public Guid MascotaID { get; set; }
    public string? Descripcion { get; set; }
    public string? InformacionContacto { get; set; }
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
}
