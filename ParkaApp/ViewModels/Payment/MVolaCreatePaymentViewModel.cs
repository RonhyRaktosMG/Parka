namespace ParkaApp.ViewModels.Payment
{
    public class MVolaCreatePaymentViewModel
    {
        public decimal Amount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public required string CarPlate { get; set; }

        public required string SelectedType { get; set; }

        public required string CustomerNumber { get; set; }

        public string? CorrelationId { get; set; }

        public string Method { get; set; } = "MVola";
    }
}