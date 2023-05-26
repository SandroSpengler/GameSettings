using GameSettings.MVVM.Pages;

namespace GameSettings;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("League", typeof(LeaugePage));
    }
}
