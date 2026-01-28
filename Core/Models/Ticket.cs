using Core.Enums;

namespace Core.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string SetNumber { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Available;
        public Guid MatchId { get; set; }
        public Match Match { get; set; }
    }
}
