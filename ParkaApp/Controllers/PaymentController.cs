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
        
        public async Task<IActionResult> Index(string search)
        {
            IEnumerable<Payment> Payments = await _repository.GetAllAsync(search);

            ViewBag.Search = search;

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
                        _ => throw new ArgumentException("Type de paiement invalide")
                    },
                    StartDate = vm.StartDate,
                    EndDate = vm.EndDate,
                };

                bool isAdded = await _repository.AddAsync(Payment, vm.CarPlate);
                if (!isAdded)
                {
                    ModelState.AddModelError("", "Le client n'existe pas. Veuillez vérifier le numéro de plaque d'immatriculation.");
                    return View(vm);
                }

                return RedirectToAction(nameof(Index));
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
                if (!isUpdated)
                {
                    ModelState.AddModelError("", "La mise à jour du paiement a échoué.");
                    return View(Payment);
                }

                return RedirectToAction(nameof(Index));
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
                ModelState.AddModelError("", "La suppression du paiement a échoué.");
                
                Payment? Payment = await _repository.GetByIdAsync(id);
                return View(Payment);
            }

            return RedirectToAction(nameof(Index));
        }



        // Statistics
        public async Task<IActionResult> Statistics(DateTime? startDate, DateTime? endDate)
        {
            DateTime actualStartDate = startDate ?? DateTime.Now.AddMonths(-1);
            DateTime actualEndDate = endDate ?? DateTime.Now.AddDays(1);
            
            
            var revenue = await _repository.GetTotalRevenuePerDateAsync(actualStartDate, actualEndDate);

            PaymentStatisticViewModel vm = new PaymentStatisticViewModel
            {
                StartDate = actualStartDate,
                EndDate = actualEndDate,
                TotalRevenuePerDate = revenue
            };

            return View(vm);
        }
    }
}