using Microsoft.AspNetCore.Mvc;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;
using ParkaApp.Services.Interfaces;
using ParkaApp.ViewModels.Payment;

namespace ParkaApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _repository;
        private readonly IMVolaService _mvolaService;
        
        public PaymentController(IPaymentRepository repository, IMVolaService mvolaService)
        {
            _repository = repository;
            _mvolaService = mvolaService;
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
    
    
        // MVola
        public async Task<IActionResult> MVolaPayment()
        {
            return View();
        }

        [HttpPost, ActionName("MVolaPayment")]
        public async Task<IActionResult> MVolaPaymentConfirmed()
        {            
            string customerNumber = "0343500004";
            int amount = 7000;



            string? serverId = await _mvolaService.MerchantPayAsync(
                customerNumber: customerNumber,
                amount: amount.ToString()
            );

            Console.WriteLine("Server ID: " + serverId);

            if (string.IsNullOrEmpty(serverId))
            {
                return BadRequest(new
                {
                    message = "Erreur lors de l'initiation du paiement MVola"
                });
            }

            return RedirectToAction(nameof(MVolaProcessing), new MVolaProcessingViewModel { CorrelationId = serverId });
        }
    
    
        // MVola Processing
        public async Task<IActionResult> MVolaProcessing(MVolaProcessingViewModel vm)
        {
            return View(vm);
        }

        // =========================
        // CHECK STATUS PAYMENT
        // =========================
        [HttpGet("Payment/mvola/status/{correlationId}")]
        public async Task<IActionResult> MVolaGetStatus(string correlationId)
        {
            if (string.IsNullOrEmpty(correlationId))
            {
                return BadRequest(new
                {
                    message = "CorrelationId is required"
                });
            }

            try
            {
                var status = await _mvolaService.GetStatusAsync(
                    correlationId
                );
                return Ok(new
                {
                    correlationId,
                    status
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error while checking payment status",
                    error = ex.Message
                });
            }
        }
    }
}