using Microsoft.AspNetCore.Mvc;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;



namespace ParkaApp.Controllers
{
    public class AreaController : Controller
    {
        private readonly IAreaRepository _repository;
        
        
        public AreaController(IAreaRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<IActionResult> Index()
        {
            IEnumerable<Area> areas = await _repository.GetAllAsync();
            return View(areas);
        }


        // Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Area area)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(area);
                return RedirectToAction(nameof(Index));
            }   

            return View(area);
        }      



        // Edit
        public async Task<IActionResult> Edit(int id)
        {
            Area? area = await _repository.GetByIdAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            return View(area);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Area area)
        {
            if (id != area.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(area);
                return RedirectToAction(nameof(Index));
            }

            return View(area);
        }



        // Delete
        public async Task<IActionResult> Delete(int id)
        {
            Area? area = await _repository.GetByIdAsync(id);
            if (area == null)           
            {
                return NotFound();  
            }
            return View(area);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Area? area = await _repository.GetByIdAsync(id);
            if (area == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        // Details
        public async Task<IActionResult> Details(int id)
        {
            var area = await _repository.GetAreaWithPlacesAsync(id);
            
            if (area == null)
                return NotFound();

            return View(area);
        }
    }
}