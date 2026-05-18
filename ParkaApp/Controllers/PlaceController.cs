using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;



namespace ParkaApp.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IPlaceRepository _repository;
        
        
        public PlaceController(IPlaceRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<IActionResult> Index()
        {
            IEnumerable<Place> Places = await _repository.GetAllAsync();
            return View(Places);
        }


        // Create
        public async Task<IActionResult> Create(int areaId)
        {
            Place place = new Place
            {
                AreaId = areaId,
                Status = PlaceStatus.Available
            };


            return View(place);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Place Place)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(Place);
                return RedirectToAction(nameof(Index));
            }   

            return View(Place);
        }      



        // Edit
        public async Task<IActionResult> Edit(int id)
        {
            Place? Place = await _repository.GetByIdAsync(id);
            if (Place == null)
            {
                return NotFound();
            }
            return View(Place);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Place Place)
        {
            if (id != Place.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(Place);
                return RedirectToAction(nameof(Index));
            }

            return View(Place);
        }



        // Delete
        public async Task<IActionResult> Delete(int id)
        {
            Place? Place = await _repository.GetByIdAsync(id);
            if (Place == null)           
            {
                return NotFound();  
            }
            return View(Place);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Place? Place = await _repository.GetByIdAsync(id);
            if (Place == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}