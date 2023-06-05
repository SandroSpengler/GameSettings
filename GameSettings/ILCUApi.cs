using GameSettings.MVVM.Models;
using RestEase;
using System.Net.Http.Headers;

namespace GameSettings
{
    public interface ILCUApi
    {
        [Header("Authorization")]
        AuthenticationHeaderValue Authorization { get; set; }

        [Get("/lol-summoner/v1/current-summoner")]
        Task<Summoner> GetCurrentSummoner();
    }
}
