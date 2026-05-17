using FindingPawls.Models;
using FindingPawls.Services.Auth;
using System.Net.Http.Json;

namespace FindingPawls.Services.Api;

// Servicio para operaciones de mascotas del usuario autenticado.
public class MascotaApiService : BaseApiService
{
    public MascotaApiService(HttpClient httpClient, AuthTokenService tokenService)
        : base(httpClient, tokenService)
    {
    }

    // Obtiene todas las mascotas registradas por el usuario autenticado.
    public async Task<(List<Mascota> Mascotas, string? Error)> ObtenerMisMascotasAsync()
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.GetAsync("mascotas");

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<List<Mascota>>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<List<Mascota>>>(JsonOptions);
                return (apiResponse?.Data ?? new(), null);
            }

            return (new(), await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (new(), ex.Message);
        }
    }

    // Obtiene el detalle de una mascota por su ID.
    public async Task<(Mascota? Mascota, string? Error)> ObtenerMascotaAsync(Guid mascotaId)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.GetAsync($"mascotas/{mascotaId}");

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<Mascota>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<Mascota>>(JsonOptions);
                return (apiResponse?.Data, null);
            }

            return (null, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    // Elimina una mascota por su ID.
    public async Task<(bool Exito, string? Error)> EliminarMascotaAsync(Guid mascotaId)
    {
        try
        {
            await PrepararAutorizacionAsync();
            HttpResponseMessage response = await _httpClient.DeleteAsync($"mascotas/{mascotaId}");

            if (response.IsSuccessStatusCode)
                return (true, null);

            return (false, await ObtenerMensajeErrorAsync(response));
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    // Crea una nueva mascota. Las fotos se suben como multipart/form-data.
    public async Task<(Mascota? Mascota, string? Error)> CrearMascotaAsync(
        CreateMascotaRequest request, FileResult? foto)
    {
        try
        {
            await PrepararAutorizacionAsync();

            using MultipartFormDataContent contenido = new();
            contenido.Add(new StringContent(request.Nombre), "Nombre");

            if (request.EspecieID.HasValue)
                contenido.Add(new StringContent(request.EspecieID.Value.ToString()), "EspecieID");
            if (request.RazaID.HasValue)
                contenido.Add(new StringContent(request.RazaID.Value.ToString()), "RazaID");
            if (request.SexoID.HasValue)
                contenido.Add(new StringContent(request.SexoID.Value.ToString()), "SexoID");
            if (!string.IsNullOrEmpty(request.Color))
                contenido.Add(new StringContent(request.Color), "Color");
            if (request.FechaNacimiento.HasValue)
                contenido.Add(new StringContent(request.FechaNacimiento.Value.ToString("yyyy-MM-dd")), "FechaNacimiento");
            if (request.Esterilizado.HasValue)
                contenido.Add(new StringContent(request.Esterilizado.Value.ToString()), "Esterilizado");
            if (!string.IsNullOrEmpty(request.NumeroMicrochip))
                contenido.Add(new StringContent(request.NumeroMicrochip), "NumeroMicrochip");
            if (!string.IsNullOrEmpty(request.Descripcion))
                contenido.Add(new StringContent(request.Descripcion), "Descripcion");

            if (foto != null)
            {
                Stream stream = await foto.OpenReadAsync();
                StreamContent streamContent = new(stream);
                streamContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                contenido.Add(streamContent, "Fotos", foto.FileName);
            }

            HttpResponseMessage response = await _httpClient.PostAsync("mascotas", contenido);

            if (response.IsSuccessStatusCode)
            {
                ApiResponse<Mascota>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<Mascota>>(JsonOptions);
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

// Servicio para catalogos: especies, razas y sexos.
public class CatalogoApiService : BaseApiService
{
    public CatalogoApiService(HttpClient httpClient, AuthTokenService tokenService)
        : base(httpClient, tokenService)
    {
    }

    public async Task<List<Especie>> ObtenerEspeciesAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("catalogos/especies");
            if (response.IsSuccessStatusCode)
            {
                ApiResponse<List<Especie>>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<List<Especie>>>(JsonOptions);
                return apiResponse?.Data ?? new();
            }
        }
        catch { }
        return new();
    }

    public async Task<List<Raza>> ObtenerRazasPorEspecieAsync(int especieId)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"catalogos/razas?EspecieID={especieId}");
            if (response.IsSuccessStatusCode)
            {
                ApiResponse<List<Raza>>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<List<Raza>>>(JsonOptions);
                return apiResponse?.Data ?? new();
            }
        }
        catch { }
        return new();
    }

    public async Task<List<Sexo>> ObtenerSexosAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("catalogos/sexos");
            if (response.IsSuccessStatusCode)
            {
                ApiResponse<List<Sexo>>? apiResponse =
                    await response.Content.ReadFromJsonAsync<ApiResponse<List<Sexo>>>(JsonOptions);
                return apiResponse?.Data ?? new();
            }
        }
        catch { }
        return new();
    }
}
