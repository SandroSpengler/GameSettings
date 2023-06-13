using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameSettings.Interfaces;
using GameSettings.MVVM.Models;
using GameSettings.MVVM.Pages;
using System.Text.Json;

namespace GameSettings.MVVM.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IMapper _mapper;

        private readonly IReleaseNoteApi _releaseNoteApi;

        private JsonSerializerOptions _jsonOptions;

        [ObservableProperty]
        private List<ReleaseNote> _releaseNotesList;

        public MainViewModel(JsonSerializerOptions jsonOptions, IMapper mapper, IReleaseNoteApi releaseNoteApi)
        {
            _mapper = mapper;

            _releaseNoteApi = releaseNoteApi;

            _jsonOptions = jsonOptions;

            _releaseNotesList = new List<ReleaseNote>();

            LoadReleaseNotes();
        }

        public async Task LoadReleaseNotes()
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


            // Check if newer releasenotes are available
            ReleaseNotesList = new List<ReleaseNote>();

            try
            {
                string releaseNotesContent = File.ReadAllText($"{basePath}\\releaseNotes.json");


                ReleaseNotesList = JsonSerializer.Deserialize<List<ReleaseNote>>(releaseNotesContent, _jsonOptions);
            }
            catch (Exception e)
            {
                //var toast = Toast.Make("Could not read locally saved Release Notes", ToastDuration.Long, 14);

                if (File.Exists($"{basePath}\\releaseNotes.json"))
                {

                    File.Delete($"{basePath}\\releaseNotes.json");
                }

            }

            List<ReleaseNote> currentReleaseNotes = await _releaseNoteApi.GetAllReleaseNotes();

            if (currentReleaseNotes == null && currentReleaseNotes.Count == 0)
            {
                return;
            }

            if (currentReleaseNotes.Count == ReleaseNotesList.Count)
            {
                return;
            }

            ReleaseNotesList = currentReleaseNotes;

            string jsonToSave = JsonSerializer.Serialize(ReleaseNotesList);

            File.WriteAllText($"{basePath}\\releaseNotes.json", jsonToSave);
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
