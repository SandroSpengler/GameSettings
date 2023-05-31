using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameSettings.MVVM.Models;

namespace GameSettings.MVVM.ViewModels
{
    [QueryProperty(nameof(ReleaseNote), nameof(ReleaseNote))]
    public partial class ReleaseNoteViewModel : ObservableObject
    {
        [ObservableProperty]
        private ReleaseNote _ReleaseNote;

        [RelayCommand]
        public Task NavigateBack() => Shell.Current.GoToAsync("../");
    }
}
