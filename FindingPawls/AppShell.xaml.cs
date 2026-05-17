using FindingPawls.Views.Extravios;
using FindingPawls.Views.Adopciones;
using FindingPawls.Views.Mascotas;
using FindingPawls.Views.Callejeros;
using FindingPawls.Views.Notificaciones;
using FindingPawls.Views.Perfil;
using FindingPawls.Views.Shared;

namespace FindingPawls;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegistrarRutas();
    }

    // Registra todas las rutas de navegacion que no son tabs directos.
    // Esto permite usar Shell.GoToAsync("///ExtravioDetailPage?ReporteId=...").
    private static void RegistrarRutas()
    {
        // Pantallas de extravios
        Routing.RegisterRoute(nameof(ExtravioDetailPage), typeof(ExtravioDetailPage));
        Routing.RegisterRoute(nameof(CreateExtravioPage), typeof(CreateExtravioPage));
        Routing.RegisterRoute(nameof(AvistamientosListPage), typeof(AvistamientosListPage));
        Routing.RegisterRoute(nameof(CreateAvistamientoPage), typeof(CreateAvistamientoPage));
        Routing.RegisterRoute(nameof(AvistamientoDetailPage), typeof(AvistamientoDetailPage));

        // Pantallas de adopciones
        Routing.RegisterRoute(nameof(AdopcionDetailPage), typeof(AdopcionDetailPage));
        Routing.RegisterRoute(nameof(CreateAdopcionPage), typeof(CreateAdopcionPage));
        Routing.RegisterRoute(nameof(MisAdopcionesPage), typeof(MisAdopcionesPage));

        // Pantallas de mascotas (accesibles desde Perfil)
        Routing.RegisterRoute(nameof(MascotasPage), typeof(MascotasPage));
        Routing.RegisterRoute(nameof(MascotaDetailPage), typeof(MascotaDetailPage));
        Routing.RegisterRoute(nameof(CreateMascotaPage), typeof(CreateMascotaPage));
        Routing.RegisterRoute(nameof(EditMascotaPage), typeof(EditMascotaPage));

        // Pantallas de reportes callejeros
        Routing.RegisterRoute(nameof(ReportesCallejerosPage), typeof(ReportesCallejerosPage));
        Routing.RegisterRoute(nameof(ReporteCallejeroDetailPage), typeof(ReporteCallejeroDetailPage));
        Routing.RegisterRoute(nameof(CreateReporteCallejeroPage), typeof(CreateReporteCallejeroPage));

        // Pantallas del perfil de usuario
        Routing.RegisterRoute(nameof(EditPerfilPage), typeof(EditPerfilPage));
        Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
        Routing.RegisterRoute(nameof(EstadisticasPage), typeof(EstadisticasPage));
        Routing.RegisterRoute(nameof(NotificacionesPreferenciasPage), typeof(NotificacionesPreferenciasPage));

        // Pantallas compartidas reutilizables
        Routing.RegisterRoute(nameof(MapPickerPage), typeof(MapPickerPage));
        Routing.RegisterRoute(nameof(ImageViewerPage), typeof(ImageViewerPage));
    }
}
