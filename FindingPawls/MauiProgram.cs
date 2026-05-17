using CommunityToolkit.Maui;
using FindingPawls.Services.Api;
using FindingPawls.Services.Auth;
using FindingPawls.ViewModels.Auth;
using FindingPawls.ViewModels.Feed;
using FindingPawls.ViewModels.Mascotas;
using FindingPawls.ViewModels.Extravios;
using FindingPawls.ViewModels.Adopciones;
using FindingPawls.ViewModels.Notificaciones;
using FindingPawls.ViewModels.Perfil;
using FindingPawls.ViewModels.Callejeros;
using FindingPawls.ViewModels.Shared;
using FindingPawls.Views.Auth;
using FindingPawls.Views.Feed;
using FindingPawls.Views.Mascotas;
using FindingPawls.Views.Extravios;
using FindingPawls.Views.Adopciones;
using FindingPawls.Views.Notificaciones;
using FindingPawls.Views.Perfil;
using FindingPawls.Views.Callejeros;
using FindingPawls.Views.Shared;
using Microsoft.Extensions.Logging;

namespace FindingPawls;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                // Tipografia Inter: descargada y almacenada en Resources/Fonts/
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                fonts.AddFont("Inter-Bold.ttf", "InterBold");
            });

        // URL base del API de Finding Pawls en Azure.
        // El trailing slash es necesario para que las rutas relativas funcionen correctamente.
        const string ApiBaseUrl = "https://app-findingpawls-c9bfc6ece9a4f3c6.southcentralus-01.azurewebsites.net/api/";

        // Registro del HttpClient compartido para todos los servicios del API.
        // Se usa un Named HttpClient con la URL base configurada.
        builder.Services.AddHttpClient("FindingPawlsApi", client =>
        {
            client.BaseAddress = new Uri(ApiBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        // Servicios de autenticacion y token (Singleton: debe ser uno en toda la app).
        builder.Services.AddSingleton<AuthTokenService>();

        // Servicios del API: cada uno recibe el HttpClient nombrado via factory.
        // Se registran como Transient porque son stateless.
        builder.Services.AddTransient<AuthApiService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new AuthApiService(
                factory.CreateClient("FindingPawlsApi"),
                sp.GetRequiredService<AuthTokenService>());
        });

        builder.Services.AddTransient<MascotaApiService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new MascotaApiService(
                factory.CreateClient("FindingPawlsApi"),
                sp.GetRequiredService<AuthTokenService>());
        });

        builder.Services.AddTransient<CatalogoApiService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new CatalogoApiService(
                factory.CreateClient("FindingPawlsApi"),
                sp.GetRequiredService<AuthTokenService>());
        });

        builder.Services.AddTransient<ExtravioApiService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new ExtravioApiService(
                factory.CreateClient("FindingPawlsApi"),
                sp.GetRequiredService<AuthTokenService>());
        });

        builder.Services.AddTransient<AdopcionApiService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new AdopcionApiService(
                factory.CreateClient("FindingPawlsApi"),
                sp.GetRequiredService<AuthTokenService>());
        });

        builder.Services.AddTransient<ReporteCallejeroApiService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new ReporteCallejeroApiService(
                factory.CreateClient("FindingPawlsApi"),
                sp.GetRequiredService<AuthTokenService>());
        });

        builder.Services.AddTransient<FeedApiService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            return new FeedApiService(
                factory.CreateClient("FindingPawlsApi"),
                sp.GetRequiredService<AuthTokenService>());
        });

        // ViewModels: Transient porque cada pagina debe tener su propia instancia.
        builder.Services.AddTransient<SplashViewModel>();
        builder.Services.AddTransient<LoginViewModel>();

        // RegisterViewModel solo necesita AuthApiService (no AuthTokenService).
        // El registro exitoso NO guarda sesion — el usuario debe hacer login manual.
        builder.Services.AddTransient<RegisterViewModel>(sp =>
            new RegisterViewModel(sp.GetRequiredService<AuthApiService>()));
        builder.Services.AddTransient<FeedViewModel>();
        builder.Services.AddTransient<MascotasViewModel>();
        builder.Services.AddTransient<MascotaDetailViewModel>();
        builder.Services.AddTransient<CreateMascotaViewModel>();
        builder.Services.AddTransient<ExtraviosViewModel>();
        builder.Services.AddTransient<ExtravioDetailViewModel>();
        builder.Services.AddTransient<CreateExtravioViewModel>();
        builder.Services.AddTransient<AvistamientosListViewModel>();
        builder.Services.AddTransient<CreateAvistamientoViewModel>();
        builder.Services.AddTransient<AvistamientoDetailViewModel>();
        builder.Services.AddTransient<AdopcionesViewModel>();
        builder.Services.AddTransient<AdopcionDetailViewModel>();
        builder.Services.AddTransient<CreateAdopcionViewModel>();
        builder.Services.AddTransient<MisAdopcionesViewModel>();
        builder.Services.AddTransient<NotificacionesViewModel>();
        builder.Services.AddTransient<NotificacionesPreferenciasViewModel>();
        builder.Services.AddTransient<PerfilViewModel>();
        builder.Services.AddTransient<EditPerfilViewModel>();
        builder.Services.AddTransient<ChangePasswordViewModel>();
        builder.Services.AddTransient<EstadisticasViewModel>();
        builder.Services.AddTransient<ReportesCallejerosViewModel>();
        builder.Services.AddTransient<ReporteCallejeroDetailViewModel>();
        builder.Services.AddTransient<CreateReporteCallejeroViewModel>();
        builder.Services.AddTransient<MapPickerViewModel>();
        builder.Services.AddTransient<ImageViewerViewModel>();

        // Views (Pages): Transient para que cada navegacion cree una instancia nueva.
        builder.Services.AddTransient<SplashPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<FeedPage>();
        builder.Services.AddTransient<MascotasPage>();
        builder.Services.AddTransient<MascotaDetailPage>();
        builder.Services.AddTransient<CreateMascotaPage>();
        builder.Services.AddTransient<EditMascotaPage>();
        builder.Services.AddTransient<ExtraviosPage>();
        builder.Services.AddTransient<ExtravioDetailPage>();
        builder.Services.AddTransient<CreateExtravioPage>();
        builder.Services.AddTransient<AvistamientosListPage>();
        builder.Services.AddTransient<CreateAvistamientoPage>();
        builder.Services.AddTransient<AvistamientoDetailPage>();
        builder.Services.AddTransient<AdopcionesPage>();
        builder.Services.AddTransient<AdopcionDetailPage>();
        builder.Services.AddTransient<CreateAdopcionPage>();
        builder.Services.AddTransient<MisAdopcionesPage>();
        builder.Services.AddTransient<NotificacionesPage>();
        builder.Services.AddTransient<NotificacionesPreferenciasPage>();
        builder.Services.AddTransient<PerfilPage>();
        builder.Services.AddTransient<EditPerfilPage>();
        builder.Services.AddTransient<ChangePasswordPage>();
        builder.Services.AddTransient<EstadisticasPage>();
        builder.Services.AddTransient<ReportesCallejerosPage>();
        builder.Services.AddTransient<ReporteCallejeroDetailPage>();
        builder.Services.AddTransient<CreateReporteCallejeroPage>();
        builder.Services.AddTransient<MapPickerPage>();
        builder.Services.AddTransient<ImageViewerPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
