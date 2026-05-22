namespace ParkaApp.ViewModels.Occupation
{
    public class AssignPlaceViewModel
    {
        public int PlaceId { get; set; }

        public int AreaId { get; set; }
        public DateTime EntryTime { get; set; }

        public string CarPlate { get; set; } = string.Empty;
    }
}