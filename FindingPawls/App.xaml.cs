using FindingPawls.Views.Auth;
using FindingPawls.Services.Auth;

namespace FindingPawls;

public partial class App : Application
{
    private readonly AuthTokenService _tokenService;
    private readonly IServiceProvider _services;

    public App(AuthTokenService tokenService, IServiceProvider services)
    {
        InitializeComponent();
        _tokenService = tokenService;
        _services = services;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // La SplashPage siempre se muestra al iniciar.
        // Como los tokens son solo en memoria, ExisteSesionAsync() siempre
        // retorna false en un inicio limpio, por lo que el Splash siempre
        // redirigira al Login.
        SplashPage splashPage = new(_tokenService, _services);
        return new Window(new NavigationPage(splashPage));
    }
}