namespace ParkaApp.Models
{
    public class Client
    {
        public int Id { get; set; }

        public required string CarPlate { get; set; } 

        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }

        public bool IsGuest { get; set; }

    }
}