using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;



namespace ParkaApp.Controllers
{
    public class OccupationController : Controller
    {
        private readonly IOccupationRepository _repository;
        
        
        public OccupationController(IOccupationRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<IActionResult> Index()
        {
            IEnumerable<Occupation> Occupations = await _repository.GetAllAsync();
            return View(Occupations);
        }


        // Create
        public async Task<IActionResult> Create(int clientId, int placeId)
        {
            Occupation Occupation = new Occupation
            {
                ClientId = clientId,
                PlaceId = placeId
            };


            return View(Occupation);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Occupation Occupation)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(Occupation);
                return RedirectToAction(nameof(Index));
            }   

            return View(Occupation);
        }      



        // Edit
        public async Task<IActionResult> Edit(int id)
        {
            Occupation? Occupation = await _repository.GetByIdAsync(id);
            if (Occupation == null)
            {
                return NotFound();
            }
            return View(Occupation);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Occupation Occupation)
        {
            if (id != Occupation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(Occupation);
                return RedirectToAction(nameof(Index));
            }

            return View(Occupation);
        }



        // Delete
        public async Task<IActionResult> Delete(int id)
        {
            Occupation? Occupation = await _repository.GetByIdAsync(id);
            if (Occupation == null)           
            {
                return NotFound();  
            }
            return View(Occupation);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Occupation? Occupation = await _repository.GetByIdAsync(id);
            if (Occupation == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}