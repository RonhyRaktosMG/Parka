namespace ParkaApp.Models
{
    public class Occupation
    {
        public int Id { get; set; }

        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }

        // Place
        public int PlaceId { get; set; }
        public Place? Place { get; set; }

        // Client
        public int ClientId { get; set; }
        public Client? Client { get; set; }
    }
}