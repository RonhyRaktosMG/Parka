using Microsoft.AspNetCore.Mvc;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;

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
        public async Task<IActionResult> Create(Payment Payment)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(Payment);
                return RedirectToAction(nameof(Index));
            }   

            return View(Payment);
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
                await _repository.UpdateAsync(Payment);
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
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}