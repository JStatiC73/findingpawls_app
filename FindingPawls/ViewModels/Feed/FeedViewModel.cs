using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FindingPawls.Models;
using FindingPawls.Services.Api;
using FindingPawls.Services.Auth;
using System.Collections.ObjectModel;

namespace FindingPawls.ViewModels.Feed;

// ViewModel del Feed principal.
// Carga items combinados (extravios + adopciones + callejeros) cercanos al usuario.
// Soporta: filtrado por tipo, pull-to-refresh y paginacion incremental.
public partial class FeedViewModel : ObservableObject
{
    private readonly FeedApiService _feedService;
    private readonly AuthTokenService _tokenService;

    // Coordenadas por defecto (Guatemala City) si no hay ubicacion disponible.
    private double _latitud = 14.6349;
    private double _longitud = -90.5069;
    private int _paginaActual = 1;
    private bool _hayMasPaginas = true;

    public ObservableCollection<FeedItem> Items { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MostrarLista))]
    private bool _isBusy;
    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private bool _isLoadingMore;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MostrarLista))]
    private bool _mostrarVacio;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MostrarLista))]
    private bool _mostrarError;

    // MostrarLista = true cuando hay items que mostrar (no hay error ni estado vacio ni carga inicial).
    // Controla la visibilidad del RefreshView para que NO tape los botones de estados alternativos.
    // Durante IsRefreshing (pull-to-refresh) la lista sigue visible para mostrar los items previos.
    public bool MostrarLista => !IsBusy && !MostrarVacio && !MostrarError;

    // Nombre del usuario logueado para el saludo personalizado.
    [ObservableProperty] private string _nombreUsuario = string.Empty;

    // Filtro activo: null = todos | "Extravio" | "Adopcion" | "Callejero"
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FiltroTodos))]
    [NotifyPropertyChangedFor(nameof(FiltroExtravios))]
    [NotifyPropertyChangedFor(nameof(FiltroAdopciones))]
    [NotifyPropertyChangedFor(nameof(FiltroCallejeros))]
    private string? _filtroActivo;

    public bool FiltroTodos => FiltroActivo is null;
    public bool FiltroExtravios => FiltroActivo == "Extravio";
    public bool FiltroAdopciones => FiltroActivo == "Adopcion";
    public bool FiltroCallejeros => FiltroActivo == "Callejero";

    public FeedViewModel(FeedApiService feedService, AuthTokenService tokenService)
    {
        _feedService = feedService;
        _tokenService = tokenService;
    }

    // Carga inicial: obtiene nombre del usuario, ubicacion y primera pagina del feed.
    [RelayCommand]
    public async Task InicializarAsync()
    {
        if (IsBusy) return;

        // Carga el nombre del usuario desde el token en memoria para el saludo.
        string nombre = _tokenService.ObtenerNombre();
        NombreUsuario = string.IsNullOrEmpty(nombre) ? "Usuario" : nombre.Split(' ')[0]; // Solo el primer nombre

        await CargarUbicacionAsync();
        await CargarFeedAsync(resetear: true);
    }

    // Pull-to-refresh: recarga la primera pagina.
    [RelayCommand]
    private async Task RefrescarAsync()
    {
        IsRefreshing = true;
        await CargarFeedAsync(resetear: true);
        IsRefreshing = false;
    }

    // Paginacion al scroll hasta el final.
    [RelayCommand]
    private async Task CargarMasAsync()
    {
        if (IsLoadingMore || !_hayMasPaginas || IsBusy) return;
        IsLoadingMore = true;
        await CargarFeedAsync(resetear: false);
        IsLoadingMore = false;
    }

    // Aplica filtro de tipo y recarga.
    [RelayCommand]
    private async Task AplicarFiltroAsync(string? tipo)
    {
        if (FiltroActivo == tipo) return;
        FiltroActivo = tipo;
        await CargarFeedAsync(resetear: true);
    }

    // Navega a las notificaciones.
    // Se usa /// (ruta absoluta) porque GoToAsync relativo no funciona desde tabs del Shell.
    [RelayCommand]
    private async Task VerNotificacionesAsync()
    {
        await Shell.Current.GoToAsync("///NotificacionesPage");
    }

    // Navega al detalle del item segun su tipo.
    // Usa TipoNormalizado para mapear "AnimalCallejero" del API a "Callejero" de las rutas.
    [RelayCommand]
    private async Task VerDetalleAsync(FeedItem item)
    {
        string ruta = item.TipoNormalizado switch
        {
            "Extravio" => $"///ExtravioDetailPage?id={item.PublicacionID}",
            "Adopcion" => $"///AdopcionDetailPage?id={item.PublicacionID}",
            "Callejero" => $"///ReporteCallejeroDetailPage?id={item.PublicacionID}",
            _ => string.Empty
        };

        if (!string.IsNullOrEmpty(ruta))
            await Shell.Current.GoToAsync(ruta);
    }

    // Logica central de carga del feed.
    private async Task CargarFeedAsync(bool resetear)
    {
        MostrarError = false;

        if (resetear)
        {
            IsBusy = true;
            _paginaActual = 1;
            _hayMasPaginas = true;
            Items.Clear();
        }

        (List<FeedItem> items, string? error) = await _feedService.ObtenerFeedAsync(
            _latitud, _longitud,
            radioKm: 25,
            tipo: FiltroActivo,
            pagina: _paginaActual);

        if (error is not null)
        {
            ErrorMessage = error;
            MostrarError = true;
        }
        else
        {
            foreach (FeedItem item in items)
                Items.Add(item);

            _hayMasPaginas = items.Count >= 20;
            if (_hayMasPaginas) _paginaActual++;
        }

        MostrarVacio = Items.Count == 0 && !MostrarError;
        IsBusy = false;
    }

    // Intenta obtener la ubicacion real del dispositivo.
    // Si falla usa coordenadas por defecto (Guatemala City).
    private async Task CargarUbicacionAsync()
    {
        try
        {
            GeolocationRequest request = new(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(5));
            Location? location = await Geolocation.Default.GetLocationAsync(request);
            if (location is not null)
            {
                _latitud = location.Latitude;
                _longitud = location.Longitude;
            }
        }
        catch
        {
            // Silencioso: se usan coordenadas por defecto.
        }
    }
}
