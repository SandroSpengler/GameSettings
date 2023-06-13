namespace GameSettings.MVVM.Models
{
    public class ReleaseNote
    {
        public long ReleaseId { get; set; }
        public string ReleaseNoteVersion { get; set; }
        public string Title { get; set; }
        public ReleaseSummary ReleaseSummary { get; set; }
        public List<UiChange> UiChange { get; set; }
    }

    public class ReleaseSummary
    {
        public long SummaryId { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
    }

    public class UiChange
    {
        public long UiId { get; set; }
        public string Name { get; set; }
        public List<AdjustedElements> AdjustedElements { get; set; }
    }

    public class AdjustedElements
    {
        public long AeId { get; set; }
        public string Description { get; set; }
    }
}
