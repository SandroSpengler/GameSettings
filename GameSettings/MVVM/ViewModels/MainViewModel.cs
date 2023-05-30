using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameSettings.MVVM.Models;
using System.Text.Json;

namespace GameSettings.MVVM.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<ReleaseNote> _ReleaseNotes;

        public MainViewModel()
        {
            _ReleaseNotes = new List<ReleaseNote>();

            LoadReleaseNotes();
        }

        public void LoadReleaseNotes()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            string basePath = @"C:\GameSettings";

            // ToDo
            // Check for Platform
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            if (!File.Exists($"{basePath}\\releaseNotes.json"))
            {
                return;
            }

            string releaseNotesContent = File.ReadAllText($"{basePath}\\releaseNotes.json");
            ReleaseNotes = JsonSerializer.Deserialize<List<ReleaseNote>>(releaseNotesContent, options);

        }

        [RelayCommand]
        public Task NavigateToRoute(string route)
        {
            if (route == null)
            {
                return null;
            }

            return Shell.Current.GoToAsync(route);
        }
    }
}