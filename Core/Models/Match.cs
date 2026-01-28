namespace Core.Models
{
    public class Match
    {
        public Guid Id { get; set; }
        public DateTimeOffset MatchDate { get; set; }
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public int TeamAScore { get; set; }
        public int TeamBScore { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}