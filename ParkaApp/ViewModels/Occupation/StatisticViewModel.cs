using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParkaApp.ViewModels.Occupation
{
    public class StatisticViewModel
    {
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required string AreaName { get; set; }
        public required List<SelectListItem> AreaOptions { get; set; }
        public required Dictionary<string, int> AreaStatistics { get; set; }
        public required Dictionary<string, Dictionary<string, int>> PlaceStatisticsPerArea { get; set; }
    }
}