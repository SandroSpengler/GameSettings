using GameSettings.MVVM.ViewModels;

namespace GameSettings.MVVM.Pages;

public partial class LeaguePage : ContentPage
{
    public LeaguePage(LeagueViewModel leagueMV)
    {
        BindingContext = leagueMV;
        InitializeComponent();
    }
}