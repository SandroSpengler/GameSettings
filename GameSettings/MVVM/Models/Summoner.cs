namespace GameSettings.MVVM.Models
{
    public class RerollPoints
    {
        public int CurrentPoints { get; set; }
        public int MaxRolls { get; set; }
        public int NumberOfRolls { get; set; }
        public int PointsCostToRoll { get; set; }
        public int PointsToReroll { get; set; }
    }

    public class Summoner
    {
        public int AccountId { get; set; }
        public string DisplayName { get; set; }
        public string InternalName { get; set; }
        public bool NameChangeFlag { get; set; }
        public int PercentCompleteForNextLevel { get; set; }
        public string Privacy { get; set; }
        public int ProfileIconId { get; set; }
        public string Puuid { get; set; }
        public RerollPoints RerollPoints { get; set; }
        public int SummonerId { get; set; }
        public int SummonerLevel { get; set; }
        public bool Unnamed { get; set; }
        public int XpSinceLastLevel { get; set; }
        public int XpUntilNextLevel { get; set; }
    }
}
