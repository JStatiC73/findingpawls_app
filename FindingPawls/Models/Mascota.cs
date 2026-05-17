namespace FindingPawls.Models;

// Mascota registrada por un usuario.
public class Mascota
{
    public Guid MascotaID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int? EspecieID { get; set; }
    public string? EspecieNombre { get; set; }
    public int? RazaID { get; set; }
    public string? RazaNombre { get; set; }
    public int? SexoID { get; set; }
    public string? SexoNombre { get; set; }
    public string? Color { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public bool? Esterilizado { get; set; }
    public string? NumeroMicrochip { get; set; }
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = "Activa";
    public List<FotoMascota> Fotos { get; set; } = new();
    public DateTime? FechaRegistro { get; set; }
}

// Foto asociada a una mascota.
public class FotoMascota
{
    public Guid FotoID { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool EsPrincipal { get; set; }
}

// DTO para crear una nueva mascota.
public class CreateMascotaRequest
{
    public string Nombre { get; set; } = string.Empty;
    public int? EspecieID { get; set; }
    public int? RazaID { get; set; }
    public int? SexoID { get; set; }
    public string? Color { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public bool? Esterilizado { get; set; }
    public string? NumeroMicrochip { get; set; }
    public string? Descripcion { get; set; }
}

// Especie de animal para los catalogos.
public class Especie
{
    public int EspecieID { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

// Raza de animal filtrada por especie.
public class Raza
{
    public int RazaID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int EspecieID { get; set; }
}

// Sexo del animal para catalogos.
public class Sexo
{
    public int SexoID { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
