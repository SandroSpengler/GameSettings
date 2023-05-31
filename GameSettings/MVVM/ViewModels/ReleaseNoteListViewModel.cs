using CommunityToolkit.Mvvm.ComponentModel;
using GameSettings.MVVM.Models;

namespace GameSettings.MVVM.ViewModels
{
    public partial class ReleaseNoteListViewModel : ObservableObject
    {
        public ReleaseNote ReleaseNote { get; set; }

        [ObservableProperty]
        private string _ReleaseNoteVersion;

        [ObservableProperty]
        private string _Title;

        [ObservableProperty]
        private string _Headline;

        public ReleaseNoteListViewModel()
        {

        }
    }
}
