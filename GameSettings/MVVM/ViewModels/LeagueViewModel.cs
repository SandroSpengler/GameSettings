using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GameSettings.MVVM.ViewModels
{
    public partial class LeagueViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _riotClientPath;

        public LeagueViewModel()
        {
            _riotClientPath = "Hello World";
        }

        [RelayCommand]
        public void SelectRiotClientPath()
        {
            RiotClientPath = "What a change!!!";
        }
    }
}
