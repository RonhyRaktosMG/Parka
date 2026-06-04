using Microsoft.AspNetCore.Mvc;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;
using ParkaApp.ViewModels.Client;



namespace ParkaApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientRepository _repository;
        
        
        public ClientController(IClientRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<IActionResult> Index(string search)
        {
            IEnumerable<Client> Clients = await _repository.GetAllAsync(search);

            ViewBag.Search = search;

            return View(Clients);
        }


        // Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client Client)
        {
            if (ModelState.IsValid)
            {
                bool isAdded = await _repository.AddAsync(Client);
                if (!isAdded)
                {
                    ModelState.AddModelError("", "Un client avec la même plaque d'immatriculation existe déjà.");
                    return View(Client);
                }

                return RedirectToAction(nameof(Index));
            }   

            return View(Client);
        }      



        // Edit
        public async Task<IActionResult> Edit(int id)
        {
            Client? Client = await _repository.GetByIdAsync(id);
            if (Client == null)
            {
                return NotFound();
            }
            return View(Client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Client Client)
        {
            if (id != Client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                bool isUpdated = await _repository.UpdateAsync(Client);
                if (!isUpdated)
                {
                    ModelState.AddModelError("", "Un client avec la même plaque d'immatriculation existe déjà.");
                    return View(Client);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(Client);
        }



        // Delete
        public async Task<IActionResult> Delete(int id)
        {
            Client? Client = await _repository.GetByIdAsync(id);
            if (Client == null)           
            {
                return NotFound();  
            }
            return View(Client);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            bool isDeleted = await _repository.DeleteAsync(id);
            if (!isDeleted)
            {
                ModelState.AddModelError("", "Le client n'existe pas ou n'a pas pu être supprimé.");
            }
            return RedirectToAction(nameof(Index));
        }


        // Details
        public async Task<IActionResult> Details(int id)
        {
            Client? Client = await _repository.GetByIdAsync(id);
            if (Client == null)
            {
                return NotFound();
            }

            ClientDetailsViewModel vm = new ClientDetailsViewModel
            {
                Id = Client.Id,
                CarPlate = Client.CarPlate,
                Name = Client.Name,
                PhoneNumber = Client.PhoneNumber,
                IsGuest = Client.IsGuest,
                Payments = Client.Payments,
                RemainingDays = Client.Payments.Any() ? (Client.Payments.OrderByDescending(p => p.EndDate).First().EndDate - DateTime.Now).Days : 0
            };

            return View(vm);
        }
    }
}