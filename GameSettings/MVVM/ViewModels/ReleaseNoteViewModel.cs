using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GameSettings.MVVM.ViewModels
{
    public partial class ReleaseNoteViewModel : ObservableObject
    {
        [RelayCommand]
        public Task NavigateBack() => Shell.Current.GoToAsync("../");
    }
}
