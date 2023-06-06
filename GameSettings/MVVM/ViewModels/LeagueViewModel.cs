using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameSettings.MVVM.Models;
using RestEase;
using System.Diagnostics;
using System.Text.Json;

namespace GameSettings.MVVM.ViewModels
{
    public partial class LeagueViewModel : ObservableObject
    {
        private IDispatcherTimer _lolProcessTimer;

        private IDispatcherTimer _lolLockFileTimer;

        private string _basePath;

        private string _lcuUsername;

        private string _lcuPassword;

        private string _lcuPort;

        private ILCUApi _lcuApi;

        public List<Process> RunningLoLClients { get; set; }

        [ObservableProperty]
        private string _riotApplicationPath;

        [ObservableProperty]
        private string _lolApplicationPath;

        [ObservableProperty]
        private bool _canLaunchClient;

        [ObservableProperty]
        private int _runningLoLClientCount;

        [ObservableProperty]
        private Summoner _summoner;

        public LeagueViewModel()
        {
            CanLaunchClient = false;
            _lcuUsername = "riot";
            _basePath = @"C:\GameSettings";

            Summoner = new Summoner();
            Summoner.DisplayName = "retrieving client information";
            RunningLoLClients = new List<Process>();

            _lolProcessTimer = Application.Current.Dispatcher.CreateTimer();
            _lolProcessTimer.Interval = TimeSpan.FromSeconds(1);
            _lolProcessTimer.Tick += (s, e) => CheckForRunningLoLClients();
            _lolProcessTimer.Start();

            _lolLockFileTimer = Application.Current.Dispatcher.CreateTimer();
            _lolLockFileTimer.Interval += TimeSpan.FromSeconds(5);
            _lolLockFileTimer.Tick += (s, e) => ReadLoLLockFile();
            _lolLockFileTimer.Start();

            LoadSettings();
        }

        public void LoadSettings()
        {
            List<LoLSettings> lolSettingsList;
            LoLSettings lolSettings;


            if (!File.Exists($"{_basePath}\\settings.json"))
            {
                RiotApplicationPath = "n/a";
                SaveAllLoLSettings();
            }

            string lolSettingsString = File.ReadAllText($"{_basePath}\\settings.json");

            lolSettingsList = JsonSerializer.Deserialize<List<LoLSettings>>(lolSettingsString);
            lolSettings = lolSettingsList[0];

            RiotApplicationPath = lolSettings.RiotApplicationPath != null ? lolSettings.RiotApplicationPath : "please select the riot client path";
            LolApplicationPath = lolSettings.LolApplicationPath != null ? lolSettings.LolApplicationPath : "please select the league client path";

            if (!RiotApplicationPath.Equals("please select the riot client path"))
            {
                CanLaunchClient = true;
            }
        }

        [RelayCommand]
        public async Task SelectLolApplicationDirecty(string clientType)
        {
            PickOptions pickOptions = new PickOptions();
            pickOptions.PickerTitle = $"Please select your {clientType} client";

            var result = await FilePicker.Default.PickAsync();

            if (result == null)
            {
                return;
            }

            if (clientType.Equals("Riot"))
            {
                if (!result.FileName.Contains("RiotClientService"))
                {
                    RiotApplicationPath = "incorret path please select a correct one";
                    CanLaunchClient = false;
                    return;
                }
                RiotApplicationPath = result.FullPath;
            }

            if (clientType.Equals("League"))
            {
                if (!result.FileName.Contains("LeagueClient"))
                {
                    LolApplicationPath = "incorret path please select a correct one";
                    CanLaunchClient = false;
                    return;
                }
                LolApplicationPath = result.FullPath;
            }

            SaveAllLoLSettings();
            CanLaunchClient = true;
        }

        [RelayCommand]
        public void SaveAllLoLSettings()
        {
            List<LoLSettings> lolSettingsList = new List<LoLSettings>();
            LoLSettings lolSettings = new LoLSettings();

            lolSettings.RiotApplicationPath = RiotApplicationPath;
            lolSettings.LolApplicationPath = LolApplicationPath;

            lolSettingsList.Add(lolSettings);

            string jsonToSave = JsonSerializer.Serialize(lolSettingsList);
            File.WriteAllText($"{_basePath}\\settings.json", jsonToSave);
        }


        [RelayCommand]
        private void StartMulticlient()
        {
            List<string> startParams = new List<string>
            {
                "--launch-product=league_of_legends",
                "--launch-patchline=live",
                "--allow-multiple-clients",
                //"--app-port=1337",
                //"--mode unattended"
            };

            if (RunningLoLClientCount > 0)
            {
                Process processMultiClient = Process.Start(RiotApplicationPath, $"{startParams[0]} {startParams[1]} {startParams[2]}");

                RunningLoLClients.Add(processMultiClient);

                return;
            }

            Process processSingleClient = Process.Start(RiotApplicationPath, $"{startParams[0]} {startParams[1]}");

            RunningLoLClients.Add(processSingleClient);
        }

        private void CheckForRunningLoLClients()
        {
            RunningLoLClientCount = 0;
            RunningLoLClients = new List<Process>();

            Process[] allProcesses = Process.GetProcesses();

            foreach (Process process in allProcesses)
            {
                if (RunningLoLClients.Any(p => p.Id == process.Id))
                {
                    continue;
                }

                if (process.ProcessName.Equals("LeagueClient")
                    //|| process.ProcessName.Equals("RiotClientUx")
                    )
                {
                    RunningLoLClients.Add(process);
                }
            }

            RunningLoLClientCount = RunningLoLClients.Count;
        }

        private void ReadLoLLockFile()
        {
            if (RunningLoLClientCount == 0)
            {
                return;
            }

            if (LolApplicationPath == null
                || LolApplicationPath.Equals("n/a")
                || !LolApplicationPath.Contains(@"\")
                )
            {
                LolApplicationPath = "please select the riot client path";
                return;
            }


            // Needs to remove the League.exe from the Path
            string[] splittedLoLPath = LolApplicationPath.Split(@"\");
            string[] pathWithoutExe = splittedLoLPath.Take(splittedLoLPath.Length - 1).ToArray();
            string finalPath = string.Join("/", pathWithoutExe);

            using FileStream fs = File.Open($"{finalPath}/lockfile", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using StreamReader rdr = new StreamReader(fs);

            string lockFileContent = rdr.ReadToEnd();

            string[] splitLCUApiProperties = lockFileContent.Split(":");
            _lcuPort = splitLCUApiProperties[2];
            _lcuPassword = splitLCUApiProperties[3];

            var authHeaderBytes = System.Text.Encoding.UTF8.GetBytes($"{_lcuUsername}:{_lcuPassword}");
            var authHeaderString = Convert.ToBase64String(authHeaderBytes);

            var handler = new HttpClientHandler();

            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            var client = new HttpClient(handler);
            client.BaseAddress = new Uri($"https://127.0.0.1:{_lcuPort}/");

            _lcuApi = RestClient.For<ILCUApi>(client);
            _lcuApi.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeaderString);

            Task.Run(() => loadClientSettings());
        }

        private async Task loadClientSettings()
        {
            try
            {
                Summoner currentSummoner = await _lcuApi.GetCurrentSummoner();

                if (currentSummoner == null)
                {
                    return;
                }

                Summoner = currentSummoner;
            }
            catch (Exception e)
            {

                //throw e;
            }

            return;

        }

        [RelayCommand]
        private void CloseAllClients()
        {
            foreach (Process lolClient in RunningLoLClients)
            {
                lolClient.Kill();
            }
        }

    }
}
