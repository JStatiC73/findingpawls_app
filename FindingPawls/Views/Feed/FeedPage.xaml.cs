using FindingPawls.ViewModels.Feed;

namespace FindingPawls.Views.Feed;

public partial class FeedPage : ContentPage
{
    private readonly FeedViewModel _viewModel;

    public FeedPage(FeedViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // Inicia la carga del feed cuando la pagina es visible por primera vez.
    // Se usa OnAppearing en lugar del constructor para que el ViewModel ya
    // este completamente inicializado por DI cuando se llama al API.
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Solo carga si el feed esta vacio (evita recarga al volver de un detalle).
        if (_viewModel.Items.Count == 0)
            await _viewModel.InicializarCommand.ExecuteAsync(null);
    }
}
