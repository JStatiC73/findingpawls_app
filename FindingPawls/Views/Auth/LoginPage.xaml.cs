using FindingPawls.ViewModels.Auth;

namespace FindingPawls.Views.Auth;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        // IMPORTANTE: Los comandos de navegacion se asignan ANTES de BindingContext.
        // Si se asignan despues, el binding XAML ya evaluo IrARegistroCommand como null
        // y como no hay INotifyPropertyChanged, el boton no se actualiza.
        viewModel.IrARegistroCommand = new Command(async () =>
        {
            // Resuelve el RegisterViewModel desde DI al momento de navegar.
            IServiceProvider services = IPlatformApplication.Current!.Services;
            RegisterViewModel registerVm = services.GetRequiredService<RegisterViewModel>();
            RegisterPage registerPage = new(registerVm);
            await Navigation.PushModalAsync(registerPage, animated: true);
        });

        InitializeComponent();
        BindingContext = viewModel;
    }
}
