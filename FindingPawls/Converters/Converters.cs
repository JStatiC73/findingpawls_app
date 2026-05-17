using System.Globalization;

namespace FindingPawls.Converters;

// Convierte un valor booleano al su inverso.
// Util para deshabilitar/habilitar controles de forma cruzada en XAML.
public class InvertBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b ? !b : false;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b ? !b : false;
}

// Convierte un string o valor nulo al estado de visibilidad.
// Visible si el valor NO es nulo ni cadena vacia.
public class NotNullOrEmptyToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is string s ? !string.IsNullOrEmpty(s) : value != null;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

// Convierte el estado de una mascota a un color de badge.
// Los estados son: "Activa", "Extraviada", "EnAdopcion".
public class EstadoMascotaColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() switch
        {
            "Activa" => Color.FromArgb("#388E3C"),
            "Extraviada" => Color.FromArgb("#D32F2F"),
            "EnAdopcion" => Color.FromArgb("#1976D2"),
            _ => Color.FromArgb("#9E9E9E")
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

// Convierte el tipo de item del feed a un color de badge.
public class TipoFeedColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() switch
        {
            "Extravio" => Color.FromArgb("#D32F2F"),
            "Adopcion" => Color.FromArgb("#1976D2"),
            "Callejero" => Color.FromArgb("#F5A623"),
            _ => Color.FromArgb("#2E7D5E")
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

// Convierte una distancia en km a texto legible.
// Si la distancia es menor a 1 km muestra metros.
public class DistanciaTextoConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double km)
        {
            if (km < 1.0)
                return $"{(int)(km * 1000)} m";
            return $"{km:F1} km";
        }
        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

// Convierte una fecha a texto relativo: "hace 2 horas", "hace 3 dias".
public class FechaRelativaConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime fecha)
        {
            TimeSpan diferencia = DateTime.Now - fecha;

            if (diferencia.TotalMinutes < 1) return "Ahora mismo";
            if (diferencia.TotalMinutes < 60) return $"Hace {(int)diferencia.TotalMinutes} min";
            if (diferencia.TotalHours < 24) return $"Hace {(int)diferencia.TotalHours} h";
            if (diferencia.TotalDays < 30) return $"Hace {(int)diferencia.TotalDays} dias";
            return fecha.ToString("dd/MM/yyyy");
        }
        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

// Convierte un valor booleano a visibilidad: true = Visible, false = Collapsed.
public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && b;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

// Convierte una URL de imagen a ImageSource con manejo de nulos.
// Si la URL es nula o vacia, retorna null para que el Image no se muestre.
public class UrlToImageSourceConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string url && !string.IsNullOrEmpty(url))
            return ImageSource.FromUri(new Uri(url));
        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

// Convierte la cantidad de insignias a la visibilidad de la seccion de insignias.
public class CountToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is int count && count > 0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

// Convierte el estado de visibilidad de contrasena a un icono de ojo Unicode.
// MostrarContrasena = false (oculta)  → 👁 (ojo abierto: "toca para ver")
// MostrarContrasena = true  (visible) → 🙈 (ojo tapado: "toca para ocultar")
// Ambos caracteres estan disponibles nativamente en Android e iOS sin fonts extra.
public class BoolToPasswordIconConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool mostrar && mostrar ? "🙈" : "👁";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
