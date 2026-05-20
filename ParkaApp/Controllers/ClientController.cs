using Microsoft.AspNetCore.Mvc;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;



namespace ParkaApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientRepository _repository;
        
        
        public ClientController(IClientRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<IActionResult> Index()
        {
            IEnumerable<Client> Clients = await _repository.GetAllAsync();
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
                    ModelState.AddModelError("", "A client with the same car plate already exists.");
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
                    ModelState.AddModelError("", "A client with the same car plate already exists.");
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
                ModelState.AddModelError("", "The client does not exist or could not be deleted.");
            }
            return RedirectToAction(nameof(Index));
        }

    }
}