using CommunityToolkit.Mvvm.ComponentModel;
using FindingPawls.Services.Auth;

namespace FindingPawls.ViewModels.Auth;

// ViewModel de la pantalla de splash inicial.
// Usa propiedades parciales (CommunityToolkit.Mvvm 8.3+) para compatibilidad AOT.
public partial class SplashViewModel : ObservableObject
{
    private readonly AuthTokenService _tokenService;

    public SplashViewModel(AuthTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    // Verifica el token y retorna true si el usuario tiene sesion activa valida.
    public async Task<bool> VerificarSesionAsync()
    {
        // Espera minima para que el splash se vea brevemente.
        await Task.Delay(1500);

        bool existeSesion = await _tokenService.ExisteSesionAsync();
        if (!existeSesion)
            return false;

        return await _tokenService.TokenEsValidoAsync();
    }
}
