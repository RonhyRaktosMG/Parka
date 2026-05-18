namespace ParkaApp.Models
{

    public enum PaymentType
    {
        Hourly,
        Daily,
        Monthly
    }


    public class Payment
    {
        public int Id { get; set; }

        public double Amount { get; set; }
        public PaymentType Type { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Client
        public int ClientId { get; set; }
        public Client? Client { get; set; }
    }
}