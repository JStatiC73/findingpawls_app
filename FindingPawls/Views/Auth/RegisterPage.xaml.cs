using FindingPawls.ViewModels.Auth;

namespace FindingPawls.Views.Auth;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
    {
        // Comandos que requieren Navigation stack — asignados ANTES de InitializeComponent.
        viewModel.CerrarCommand = new Command(async () => await Navigation.PopModalAsync(animated: true));
        viewModel.IrALoginCommand = new Command(async () => await Navigation.PopModalAsync(animated: true));

        // Callback de registro exitoso:
        // 1. Muestra dialogo de confirmacion con DisplayAlert (nativo del sistema).
        // 2. Cierra el modal de registro y vuelve al Login.
        // El usuario DEBE iniciar sesion manualmente con sus nuevas credenciales.
        viewModel.OnRegistroExitosoAsync = async () =>
        {
            await DisplayAlertAsync(
                "¡Cuenta creada!",
                "Tu cuenta fue registrada exitosamente.\n\n" +
                "Ya puedes iniciar sesion con tu correo y contraseña.",
                "Ir al Login");

            // Cierra el modal de registro: el usuario queda en LoginPage.
            await Navigation.PopModalAsync(animated: true);
        };

        InitializeComponent();
        BindingContext = viewModel;
    }
}
