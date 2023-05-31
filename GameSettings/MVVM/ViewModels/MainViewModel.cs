using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameSettings.MVVM.Models;
using GameSettings.MVVM.Pages;
using System.Text.Json;

namespace GameSettings.MVVM.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IMapper _mapper;

        private JsonSerializerOptions _JsonOptions;

        [ObservableProperty]
        private List<ReleaseNote> _ReleaseNotes;

        public MainViewModel(JsonSerializerOptions jsonOptions, IMapper mapper)
        {
            _mapper = mapper;

            _JsonOptions = jsonOptions;

            _ReleaseNotes = new List<ReleaseNote>();

            LoadReleaseNotes();
        }

        public void LoadReleaseNotes()
        {
            string basePath = @"C:\GameSettings";

#if !ANDROID && !IOS && !MACCATALYST

            //ToDo
            //Check for Platform
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
#endif
            if (!File.Exists($"{basePath}\\releaseNotes.json"))
            {
                return;
            }

            ReleaseNotes = new List<ReleaseNote>();

            string releaseNotesContent = File.ReadAllText($"{basePath}\\releaseNotes.json");
            ReleaseNotes = JsonSerializer.Deserialize<List<ReleaseNote>>(releaseNotesContent, _JsonOptions);


        }

        [RelayCommand]
        public Task NavigateToRoute(ReleaseNote selectedReleaseNote)
        {

            // ToDo
            // NotificationToast on error
            if (selectedReleaseNote == null)
            {
                return null;
            }

            return Shell.Current.GoToAsync(nameof(ReleaseNotePage), new Dictionary<string, object>
            {
                ["ReleaseNote"] = selectedReleaseNote
            });
        }

    }
}
