
namespace ParkaApp.ViewModels.Client
{
    public class ClientDetailsViewModel
    {
        public int Id { get; set; }

        public required string CarPlate { get; set; }

        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }

        public bool IsGuest { get; set; }

        // Payment
        public List<ParkaApp.Models.Payment> Payments { get; set; } = [];

        public int RemainingDays { get; set; }
    }
}