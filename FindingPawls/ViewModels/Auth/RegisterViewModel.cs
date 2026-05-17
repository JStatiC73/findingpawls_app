using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FindingPawls.Models;
using FindingPawls.Services.Api;
using FindingPawls.Services.Auth;
using System.Text.RegularExpressions;

namespace FindingPawls.ViewModels.Auth;

// ViewModel de la pantalla de registro.
//
// FLUJO: El registro NO inicia sesion automaticamente.
// Tras un registro exitoso se notifica al code-behind via OnRegistroExitosoAsync
// para que muestre un dialogo de confirmacion y navegue de vuelta al Login.
// El usuario debe iniciar sesion explicitamente con sus nuevas credenciales.
public partial class RegisterViewModel : ObservableObject
{
    private readonly AuthApiService _authService;

    [ObservableProperty] private string _nombre = string.Empty;
    [ObservableProperty] private string _correo = string.Empty;
    [ObservableProperty] private string _contrasena = string.Empty;
    [ObservableProperty] private string _confirmarContrasena = string.Empty;
    [ObservableProperty] private string _telefono = string.Empty;

    // Mensajes de error inline por campo.
    [ObservableProperty] private string _errorNombre = string.Empty;
    [ObservableProperty] private string _errorCorreo = string.Empty;
    [ObservableProperty] private string _errorContrasena = string.Empty;
    [ObservableProperty] private string _errorConfirmar = string.Empty;
    [ObservableProperty] private string _errorGeneral = string.Empty;

    [ObservableProperty] private bool _mostrarErrorGeneral;
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private bool _mostrarContrasena;
    [ObservableProperty] private bool _mostrarConfirmar;

    // Comandos de navegacion asignados desde el code-behind (requieren Navigation stack).
    public System.Windows.Input.ICommand? CerrarCommand { get; set; }
    public System.Windows.Input.ICommand? IrALoginCommand { get; set; }

    // Callback invocado tras un registro exitoso.
    // El code-behind lo asigna para mostrar un dialogo y navegar al Login.
    // Se usa Func<Task> para permitir operaciones asincronas (DisplayAlert).
    public Func<Task>? OnRegistroExitosoAsync { get; set; }

    // Indicador de fortaleza de contrasena (0-4).
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FortalezaNormalizada))]
    private int _fortalezaContrasena;

    [ObservableProperty] private string _textoFortaleza = string.Empty;
    [ObservableProperty] private Color _colorFortaleza = Color.FromArgb("#DDE8E2");

    // Normalizado a 0.0-1.0 para el ProgressBar de MAUI.
    public double FortalezaNormalizada => FortalezaContrasena / 4.0;

    // Solo recibe AuthApiService — no AuthTokenService porque el registro
    // no debe guardar sesion automaticamente.
    public RegisterViewModel(AuthApiService authService)
    {
        _authService = authService;
    }

    partial void OnContrasenaChanged(string value)
    {
        ErrorContrasena = string.Empty;
        EvaluarFortalezaContrasena(value);
        if (!string.IsNullOrEmpty(ConfirmarContrasena))
            ValidarCoincidenciaContrasenas();
    }

    partial void OnConfirmarContrasenaChanged(string value) => ValidarCoincidenciaContrasenas();
    partial void OnNombreChanged(string value) => ErrorNombre = string.Empty;
    partial void OnCorreoChanged(string value) => ErrorCorreo = string.Empty;

    private void EvaluarFortalezaContrasena(string contrasena)
    {
        int puntaje = 0;
        if (contrasena.Length >= 8) puntaje++;
        if (Regex.IsMatch(contrasena, "[A-Z]")) puntaje++;
        if (Regex.IsMatch(contrasena, "[0-9]")) puntaje++;
        if (Regex.IsMatch(contrasena, "[^a-zA-Z0-9]")) puntaje++;

        FortalezaContrasena = puntaje;
        (TextoFortaleza, ColorFortaleza) = puntaje switch
        {
            0 or 1 => ("Debil", Color.FromArgb("#D32F2F")),
            2 => ("Media", Color.FromArgb("#F5A623")),
            3 => ("Buena", Color.FromArgb("#388E3C")),
            _ => ("Fuerte", Color.FromArgb("#2E7D5E"))
        };
    }

    private void ValidarCoincidenciaContrasenas()
    {
        ErrorConfirmar = Contrasena != ConfirmarContrasena
            ? "Las contrasenas no coinciden."
            : string.Empty;
    }

    private bool ValidarFormulario()
    {
        bool valido = true;

        if (string.IsNullOrWhiteSpace(Nombre) || Nombre.Trim().Length < 2)
        {
            ErrorNombre = "El nombre debe tener al menos 2 caracteres.";
            valido = false;
        }

        if (string.IsNullOrWhiteSpace(Correo) ||
            !Regex.IsMatch(Correo.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            ErrorCorreo = "Ingresa un correo electronico valido.";
            valido = false;
        }

        if (Contrasena.Length < 8)
        {
            ErrorContrasena = "La contrasena debe tener al menos 8 caracteres.";
            valido = false;
        }

        if (Contrasena != ConfirmarContrasena)
        {
            ErrorConfirmar = "Las contrasenas no coinciden.";
            valido = false;
        }

        return valido;
    }

    [RelayCommand]
    private async Task RegistrarseAsync()
    {
        if (!ValidarFormulario()) return;

        MostrarErrorGeneral = false;
        ErrorGeneral = string.Empty;
        IsBusy = true;

        try
        {
            RegisterRequest request = new()
            {
                Nombre = Nombre.Trim(),
                Correo = Correo.Trim().ToLower(),
                Contrasena = Contrasena,
                ConfirmarContrasena = ConfirmarContrasena,
                Telefono = string.IsNullOrWhiteSpace(Telefono) ? null : Telefono.Trim()
            };

            (bool exito, AuthResponse? _, string? error) = await _authService.RegistrarAsync(request);

            if (exito)
            {
                // Registro exitoso: NO guardamos sesion.
                // El callback mostrara un dialogo de confirmacion y navegara al Login.
                // El usuario debe iniciar sesion explicitamente.
                if (OnRegistroExitosoAsync is not null)
                    await OnRegistroExitosoAsync.Invoke();
            }
            else
            {
                ErrorGeneral = error ?? "No se pudo completar el registro. Intenta de nuevo.";
                MostrarErrorGeneral = true;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void AlternarContrasena() => MostrarContrasena = !MostrarContrasena;

    [RelayCommand]
    private void AlternarConfirmar() => MostrarConfirmar = !MostrarConfirmar;
}
