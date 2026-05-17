using FindingPawls.Services.Auth;
using FindingPawls.ViewModels.Auth;

namespace FindingPawls.Views.Auth;

public partial class SplashPage : ContentPage
{
    private readonly AuthTokenService _tokenService;
    private readonly IServiceProvider _services;

    public SplashPage(AuthTokenService tokenService, IServiceProvider services)
    {
        InitializeComponent();
        _tokenService = tokenService;
        _services = services;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await VerificarSesionYNavegar();
    }

    // Verifica si hay sesion activa y navega al destino correcto.
    // Este metodo se ejecuta una vez que el splash es visible en pantalla.
    private async Task VerificarSesionYNavegar()
    {
        // Delay minimo para que el usuario vea el logo brevemente.
        await Task.Delay(1800);

        bool sesionActiva = await _tokenService.ExisteSesionAsync();

        if (sesionActiva)
        {
            // Usuario con sesion guardada: navega al Shell principal.
            Application.Current!.Windows[0].Page = new AppShell();
        }
        else
        {
            // Sin sesion: navega al Login.
            // Se resuelve via IServiceProvider para que el LoginViewModel reciba sus dependencias.
            LoginViewModel loginVm = _services.GetRequiredService<LoginViewModel>();
            LoginPage loginPage = new(loginVm);
            Application.Current!.Windows[0].Page = new NavigationPage(loginPage);
        }
    }
}
