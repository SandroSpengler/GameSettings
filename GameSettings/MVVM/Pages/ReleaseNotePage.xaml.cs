using GameSettings.MVVM.ViewModels;

namespace GameSettings.MVVM.Pages;

public partial class ReleaseNotePage : ContentPage
{
    public ReleaseNotePage(ReleaseNoteViewModel releaseNoteVM)
    {
        BindingContext = releaseNoteVM;
        InitializeComponent();
    }
}