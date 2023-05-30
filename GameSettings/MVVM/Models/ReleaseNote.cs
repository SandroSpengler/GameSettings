namespace GameSettings.MVVM.Models
{
    public class ReleaseNote
    {
        public string ReleaseNoteVersion { get; set; }
        public string Title { get; set; }
        public ReleaseSummary ReleaseSummary { get; set; }
        public UserExperience UserExperience { get; set; }
    }

    public class UiChange
    {
        public string Name { get; set; }
        public List<string> AdjustedElements { get; set; }
    }

    public class UserExperience
    {
        public List<UiChange> UiChanges { get; set; }
    }

    public class ReleaseSummary
    {
        public string Headline { get; set; }
        public string Content { get; set; }
    }

}
