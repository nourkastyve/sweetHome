using Microsoft.Extensions.Logging;
namespace SweetHome;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder().UseMauiApp<App>().ConfigureFonts(fonts => { });
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
