using FindingPawls.Models;
using FindingPawls.Services.Auth;
using System.Net.Http.Json;

namespace FindingPawls.Services.Api;

// Servicio para operaciones de reportes de extravio y avistamientos.
public class ExtravioApiService : BaseApiService
{
    public ExtravioApiService(HttpClient httpClient, AuthTokenService tokenService)
        : base(httpClient, tokenService)
    {
    }

    // Obtiene reportes de extravios cercanos a una ubicacion.
    public async Task<(List<ReporteExtravio> Reportes, string? Error)> ObtenerExtraviosCercanosAsync(
        double latitud, double longitud, double radioKm = 10)
    {
        try
        {
            await PrepararAutorizacionAsync();
            string url = $"extravios?Latitud={latitud}&Longitud={longitud}&RadioKm={radioKm}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<List<ReporteExtravio>>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<List<ReporteExtravio>>>(JsonOptions);
                return (apiResponse?.Data ?? new(), null);
            }

            return (new(), await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (new(), ex.Message);
        }
    }

    // Obtiene el detalle de un reporte de extravio.
    public async Task<(ReporteExtravio? Reporte, string? Error)> ObtenerExtravioAsync(Guid reporteId)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.GetAsync($"extravios/{reporteId}");

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<ReporteExtravio>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<ReporteExtravio>>(JsonOptions);
                return (apiResponse?.Data, null);
            }

            return (null, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    // Crea un reporte de extravio para una mascota del usuario.
    public async Task<(ReporteExtravio? Reporte, string? Error)> CrearExtravioAsync(CreateExtravioRequest request)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("extravios", request, JsonOptions);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<ReporteExtravio>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<ReporteExtravio>>(JsonOptions);
                return (apiResponse?.Data, null);
            }

            return (null, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    // Cierra el reporte de extravio marcandolo como resuelto (mascota encontrada).
    public async Task<(bool Exito, string? Error)> CerrarExtravioAsync(Guid reporteId)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"extravios/{reporteId}/cerrar", new { }, JsonOptions);

            if (response.IsSuccessStatusCode)
                return (true, null);

            return (false, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    // Obtiene todos los avistamientos de un reporte de extravio.
    public async Task<(List<Avistamiento> Avistamientos, string? Error)> ObtenerAvistamientosAsync(Guid reporteId)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.GetAsync($"extravios/{reporteId}/avistamientos");

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<List<Avistamiento>>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<List<Avistamiento>>>(JsonOptions);
                return (apiResponse?.Data ?? new(), null);
            }

            return (new(), await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (new(), ex.Message);
        }
    }

    // Crea un avistamiento con foto. La foto es obligatoria segun las reglas del negocio.
    public async Task<(Avistamiento? Avistamiento, string? Error)> CrearAvistamientoAsync(
        Guid reporteId, CreateAvistamientoRequest request, FileResult foto)
    {
        try
        {
            await PrepararAutorizacionAsync();

            using MultipartFormDataContent contenido = new();
            contenido.Add(new StringContent(reporteId.ToString()), "ReporteExtravioID");

            if (request.Latitud.HasValue)
                contenido.Add(new StringContent(request.Latitud.Value.ToString()), "Latitud");
            if (request.Longitud.HasValue)
                contenido.Add(new StringContent(request.Longitud.Value.ToString()), "Longitud");
            if (!string.IsNullOrEmpty(request.Descripcion))
                contenido.Add(new StringContent(request.Descripcion), "Descripcion");

            Stream stream = await foto.OpenReadAsync();
            StreamContent streamContent = new(stream);
            streamContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            contenido.Add(streamContent, "Foto", foto.FileName);

            HttpResponseMessage response = await _httpClient.PostAsync(
                $"extravios/{reporteId}/avistamientos", contenido);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<Avistamiento>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<Avistamiento>>(JsonOptions);
                return (apiResponse?.Data, null);
            }

            return (null, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }
}

// Servicio para publicaciones de adopcion.
public class AdopcionApiService : BaseApiService
{
    public AdopcionApiService(HttpClient httpClient, AuthTokenService tokenService)
        : base(httpClient, tokenService)
    {
    }

    // Busca adopciones cercanas con filtros opcionales.
    public async Task<(List<Adopcion> Adopciones, string? Error)> BuscarAdopcionesAsync(
        AdopcionBusquedaParams filtros)
    {
        try
        {
            await PrepararAutorizacionAsync();

            List<string> queryParts = new();
            if (filtros.Latitud.HasValue) queryParts.Add($"Latitud={filtros.Latitud}");
            if (filtros.Longitud.HasValue) queryParts.Add($"Longitud={filtros.Longitud}");
            queryParts.Add($"RadioKm={filtros.RadioKm}");
            if (filtros.EspecieID.HasValue) queryParts.Add($"EspecieID={filtros.EspecieID}");
            if (filtros.SexoID.HasValue) queryParts.Add($"SexoID={filtros.SexoID}");
            queryParts.Add($"Pagina={filtros.Pagina}");
            queryParts.Add($"TamanoPagina={filtros.TamanoPagina}");

            string url = $"adopciones/buscar?{string.Join("&", queryParts)}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<List<Adopcion>>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<List<Adopcion>>>(JsonOptions);
                return (apiResponse?.Data ?? new(), null);
            }

            return (new(), await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (new(), ex.Message);
        }
    }

    // Obtiene el detalle de una adopcion.
    public async Task<(Adopcion? Adopcion, string? Error)> ObtenerAdopcionAsync(Guid adopcionId)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.GetAsync($"adopciones/{adopcionId}");

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<Adopcion>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<Adopcion>>(JsonOptions);
                return (apiResponse?.Data, null);
            }

            return (null, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    // Publica una mascota en adopcion.
    public async Task<(Adopcion? Adopcion, string? Error)> PublicarAdopcionAsync(CreateAdopcionRequest request)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("adopciones", request, JsonOptions);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<Adopcion>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<Adopcion>>(JsonOptions);
                return (apiResponse?.Data, null);
            }

            return (null, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    // Solicita adoptar una mascota.
    public async Task<(bool Exito, string? Error)> SolicitarAdopcionAsync(Guid adopcionId)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                $"adopciones/{adopcionId}/solicitar", new { }, JsonOptions);

            if (response.IsSuccessStatusCode)
                return (true, null);

            return (false, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    // Aprueba una solicitud de adopcion (solo el propietario).
    public async Task<(bool Exito, string? Error)> AprobarAdopcionAsync(Guid adopcionId)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
                $"adopciones/{adopcionId}/aprobar", new { }, JsonOptions);

            if (response.IsSuccessStatusCode)
                return (true, null);

            return (false, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    // Completa el proceso de adopcion.
    public async Task<(bool Exito, string? Error)> CompletarAdopcionAsync(Guid adopcionId)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
                $"adopciones/{adopcionId}/completar", new { }, JsonOptions);

            if (response.IsSuccessStatusCode)
                return (true, null);

            return (false, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}

// Servicio para reportes de animales callejeros.
public class ReporteCallejeroApiService : BaseApiService
{
    public ReporteCallejeroApiService(HttpClient httpClient, AuthTokenService tokenService)
        : base(httpClient, tokenService)
    {
    }

    // Obtiene reportes callejeros cercanos.
    public async Task<(List<ReporteCallejero> Reportes, string? Error)> ObtenerCercanosAsync(
        double latitud, double longitud, double radioKm = 10)
    {
        try
        {
            await PrepararAutorizacionAsync();
            string url = $"reportes-callejeros?Latitud={latitud}&Longitud={longitud}&RadioKm={radioKm}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<List<ReporteCallejero>>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<List<ReporteCallejero>>>(JsonOptions);
                return (apiResponse?.Data ?? new(), null);
            }

            return (new(), await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (new(), ex.Message);
        }
    }

    // Crea un reporte de animal callejero con foto obligatoria.
    public async Task<(ReporteCallejero? Reporte, string? Error)> CrearReporteAsync(
        CreateReporteCallejeroRequest request, FileResult foto)
    {
        try
        {
            await PrepararAutorizacionAsync();

            using MultipartFormDataContent contenido = new();
            contenido.Add(new StringContent(request.Descripcion), "Descripcion");

            if (request.Latitud.HasValue)
                contenido.Add(new StringContent(request.Latitud.Value.ToString()), "Latitud");
            if (request.Longitud.HasValue)
                contenido.Add(new StringContent(request.Longitud.Value.ToString()), "Longitud");

            Stream stream = await foto.OpenReadAsync();
            StreamContent streamContent = new(stream);
            streamContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            contenido.Add(streamContent, "Foto", foto.FileName);

            HttpResponseMessage response = await _httpClient.PostAsync("reportes-callejeros", contenido);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<ReporteCallejero>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<ReporteCallejero>>(JsonOptions);
                return (apiResponse?.Data, null);
            }

            return (null, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    // Marca un reporte callejero como resuelto.
    public async Task<(bool Exito, string? Error)> ResolverReporteAsync(Guid reporteId)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
                $"reportes-callejeros/{reporteId}/resolver", new { Resolucion = "Resuelto" }, JsonOptions);

            if (response.IsSuccessStatusCode)
                return (true, null);

            return (false, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}

// Servicio para el feed combinado del usuario.
public class FeedApiService : BaseApiService
{
    public FeedApiService(HttpClient httpClient, AuthTokenService tokenService)
        : base(httpClient, tokenService)
    {
    }

    // Obtiene el feed personalizado del usuario con publicaciones cercanas.
    // El parametro tipo acepta: null (todos), "Extravio", "Adopcion", "AnimalCallejero".
    // NOTA: el API recibe "Tipos" como lista de query params repetidos.
    public async Task<(List<FeedItem> Items, string? Error)> ObtenerFeedAsync(
        double latitud, double longitud, double radioKm = 10, string? tipo = null, int pagina = 1)
    {
        try
        {
            await PrepararAutorizacionAsync();

            // El API acepta Tipos como parametros repetidos: &Tipos=Adopcion&Tipos=Extravio
            // Si tipo es null, no se agrega filtro y el API retorna todos los tipos.
            string tipoApiParam = tipo switch
            {
                "Callejero" => "AnimalCallejero",  // Mapeo: nombre interno -> enum del API
                _ => tipo ?? string.Empty
            };

            string url = $"feed?Latitud={latitud}&Longitud={longitud}&RadioKm={radioKm}&Pagina={pagina}&TamanoPagina=20";

            if (!string.IsNullOrEmpty(tipoApiParam))
                url += $"&Tipos={tipoApiParam}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<List<FeedItem>>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<List<FeedItem>>>(JsonOptions);
                return (apiResponse?.Data ?? new(), null);
            }

            return (new(), await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (new(), ex.Message);
        }
    }
}
