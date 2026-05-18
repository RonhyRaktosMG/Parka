namespace ParkaApp.Models
{
    public class Area
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public string? Address { get; set; }


        public double Latitude { get; set; }
        public double Longitude { get; set; }


        // Place
        public List<Place> Places { get; set; } = new List<Place>();
    }

}