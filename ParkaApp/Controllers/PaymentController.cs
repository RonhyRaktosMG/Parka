using Microsoft.AspNetCore.Mvc;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;
using ParkaApp.ViewModels.Payment;

namespace ParkaApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _repository;
        
        
        public PaymentController(IPaymentRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<IActionResult> Index()
        {
            IEnumerable<Payment> Payments = await _repository.GetAllAsync();
            return View(Payments);
        }

        // Create
        public IActionResult Create()
        {
            return View();

        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Payment Payment = new Payment
                {
                    Amount = (double)vm.Amount,
                    Type = vm.SelectedType switch
                    {
                        "Hourly" => PaymentType.Hourly,
                        "Daily" => PaymentType.Daily,
                        "Monthly" => PaymentType.Monthly,
                        _ => throw new ArgumentException("Invalid payment type")
                    },
                    StartDate = vm.StartDate,
                    EndDate = vm.EndDate,
                };

                bool isAdded = await _repository.AddAsync(Payment, vm.CarPlate);
                if (isAdded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(vm);
        }        




        // Edit
        public async Task<IActionResult> Edit(int id)
        {
            Payment? Payment = await _repository.GetByIdAsync(id);
            if (Payment == null)
            {
                return NotFound();
            }
            return View(Payment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Payment Payment)
        {
            if (id != Payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                bool isUpdated = await _repository.UpdateAsync(Payment);
                if (isUpdated)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(Payment);
        }



        // Delete
        public async Task<IActionResult> Delete(int id)
        {
            Payment? Payment = await _repository.GetByIdAsync(id);
            if (Payment == null)            {
                return NotFound();
            }
            return View(Payment);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            if (!isDeleted)
            {
                ModelState.AddModelError("", "Failed to delete the payment.");
                
                Payment? Payment = await _repository.GetByIdAsync(id);
                return View(Payment);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}