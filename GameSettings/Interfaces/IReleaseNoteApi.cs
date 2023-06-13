using GameSettings.MVVM.Models;
using RestEase;

namespace GameSettings.Interfaces
{
    public interface IReleaseNoteApi
    {
        //[Header("Authorization")]
        //AuthenticationHeaderValue Authorization { get; set; }

        [Get("/releaseNotes")]
        Task<List<ReleaseNote>> GetAllReleaseNotes();
    }
}
