using MemoryGame.DataAccess;
using Microsoft.Extensions.Logging;

namespace MauiAppBlazer;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Register HighScoreRepository as a singleton or transient
        builder.Services.AddSingleton<HighScoreRepository>();
        builder.Services.AddSingleton<
            MemoryGame.Business.IHighScoreRepository,
            HighScoreRepository
        >();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
