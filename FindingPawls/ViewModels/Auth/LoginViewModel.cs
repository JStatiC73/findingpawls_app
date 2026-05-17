using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FindingPawls.Models;
using FindingPawls.Services.Api;
using FindingPawls.Services.Auth;
using System.Windows.Input;

namespace FindingPawls.ViewModels.Auth;

// ViewModel de la pantalla de login.
// Maneja el estado del formulario, la llamada al API y la navegacion post-login.
public partial class LoginViewModel : ObservableObject
{
    private readonly AuthApiService _authService;
    private readonly AuthTokenService _tokenService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PuedeIniciarSesion))]
    private string _correo = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PuedeIniciarSesion))]
    private string _contrasena = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _mostrarError;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PuedeIniciarSesion))]
    private bool _isBusy;

    [ObservableProperty]
    private bool _mostrarContrasena;

    // Comando de navegacion al registro. Se asigna desde el code-behind
    // porque requiere acceso al Navigation stack de la pagina.
    public ICommand? IrARegistroCommand { get; set; }

    // El boton de login solo se habilita si hay correo, contrasena y no esta cargando.
    public bool PuedeIniciarSesion =>
        !string.IsNullOrWhiteSpace(Correo) &&
        !string.IsNullOrWhiteSpace(Contrasena) &&
        !IsBusy;

    public LoginViewModel(AuthApiService authService, AuthTokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    // Ejecuta el login contra el API.
    // Si es exitoso, guarda la sesion y navega al Shell principal.
    [RelayCommand(CanExecute = nameof(PuedeIniciarSesion))]
    private async Task IniciarSesionAsync()
    {
        MostrarError = false;
        ErrorMessage = string.Empty;
        IsBusy = true;

        try
        {
            LoginRequest request = new()
            {
                Correo = Correo.Trim(),
                Contrasena = Contrasena
            };

            (bool exito, AuthResponse? respuesta, string? error) = await _authService.LoginAsync(request);

            if (exito && respuesta != null)
            {
                await _tokenService.GuardarSesionAsync(respuesta);

                // Navega al Shell principal reemplazando la pila de navegacion.
                // El usuario no podra volver al login con el boton Back.
                Application.Current!.Windows[0].Page = new AppShell();
            }
            else
            {
                ErrorMessage = error ?? "Correo o contrasena incorrectos.";
                MostrarError = true;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void AlternarVisibilidadContrasena()
    {
        MostrarContrasena = !MostrarContrasena;
    }

    partial void OnCorreoChanged(string value) => MostrarError = false;
    partial void OnContrasenaChanged(string value) => MostrarError = false;
}
