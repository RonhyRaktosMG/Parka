namespace ParkaApp.ViewModels.Occupation
{
    public class ReleasePlaceViewModel
    {
        public int OccupationId { get; set; }
        public int PlaceId { get; set; }
        public string CarPlate { get; set; } = string.Empty;
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }

        public decimal TotalCost
        {
            get
            {
                var duration = ExitTime - EntryTime;
                return (decimal)duration.TotalHours * 500; // Example: 500 Ar per hour
            }
        }

        public string FormattedDuration
        {
            get
            {
                var duration = ExitTime - EntryTime;
                return $"{(int)duration.TotalHours}h {duration.Minutes}m";
            }
        }
    }
}