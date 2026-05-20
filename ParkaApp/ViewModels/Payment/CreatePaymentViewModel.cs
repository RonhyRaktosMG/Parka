using Microsoft.AspNetCore.Mvc.Rendering;
using ParkaApp.Models;

namespace ParkaApp.ViewModels.Payment
{
    public class CreatePaymentViewModel
    {
        public decimal Amount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public required string CarPlate { get; set; }

        public required string SelectedType { get; set; }
    }
}