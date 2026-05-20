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
                bool isAdded = await _repository.AddAsync(Place);
                if (!isAdded)
                {   
                    ModelState.AddModelError("", "A place with the same code already exists in the selected area.");
                    return View(Place);
                }
                
                return RedirectToAction("Details", "Area", new { id = Place.AreaId });
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

            if (ModelState.IsValid)
            {
                bool isUpdated = await _repository.UpdateAsync(Place);
                if (!isUpdated)
                {
                    ModelState.AddModelError("", "A place with the same code already exists in the selected area.");
                    return View(Place);
                }

                return RedirectToAction("Details", "Area", new { id = Place.AreaId });
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

            bool isDeleted = await _repository.DeleteAsync(id);
            if (!isDeleted)
            {
                ModelState.AddModelError("", "A place with the same code already exists in the selected area.");
                return View(Place);
            }

            return RedirectToAction("Details", "Area", new { id = Place!.AreaId });
        }
    }
}