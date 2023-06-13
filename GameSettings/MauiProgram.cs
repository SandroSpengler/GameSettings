using CommunityToolkit.Maui;
using GameSettings.Interfaces;
using GameSettings.MVVM.Pages;
using GameSettings.MVVM.ViewModels;
using Microsoft.Extensions.Logging;
using RestEase.HttpClientFactory;
using System.Text.Json;
namespace GameSettings;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        });

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddRestEaseClient<IReleaseNoteApi>("https://releasenote.axfert.com/api");

        builder.Services.AddTransient<LeaguePage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ReleaseNotePage>();

        builder.Services.AddTransient<LeagueViewModel>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<ReleaseNoteViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
