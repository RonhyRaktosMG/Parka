namespace ParkaApp.ViewModels.Occupation
{
    public class ReleasePlaceViewModel
    {
        public int OccupationId { get; set; }
        public int PlaceId { get; set; }
        public string CarPlate { get; set; } = string.Empty;
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }

        public double TotalCost { get; set; }

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