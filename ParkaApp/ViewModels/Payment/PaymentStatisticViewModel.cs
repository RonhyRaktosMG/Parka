namespace ParkaApp.ViewModels.Payment
{
    public class PaymentStatisticViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Dictionary<string, double> TotalRevenuePerDate { get; set; } = new Dictionary<string, double>();
    }
}