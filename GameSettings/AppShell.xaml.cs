using GameSettings.MVVM.Pages;

namespace GameSettings;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(LeaguePage), typeof(LeaguePage));
        Routing.RegisterRoute(nameof(ReleaseNotePage), typeof(ReleaseNotePage));
    }
}
