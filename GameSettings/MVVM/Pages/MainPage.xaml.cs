using GameSettings.MVVM.ViewModels;

namespace GameSettings;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel mainVM)
    {
        BindingContext = mainVM;
        InitializeComponent();
    }
}