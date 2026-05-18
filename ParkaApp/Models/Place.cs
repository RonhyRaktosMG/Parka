namespace ParkaApp.Models
{

    public enum PlaceStatus
    {
        Available,
        Occupied,
        Reserved
    }

    public class Place
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public PlaceStatus Status { get; set; }


        // Area
        public int AreaId { get; set; }
        public Area? Area { get; set; }
    }
}