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
                    ModelState.AddModelError("", "Une erreur est survenue lors de la création de la place. Assurez-vous qu'une place avec le même code n'existe pas déjà dans la zone sélectionnée ou que la zone existe.");
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
                    ModelState.AddModelError("", "Une erreur est survenue lors de la mise à jour de la place. Assurez-vous qu'une place avec le même code n'existe pas déjà dans la zone sélectionnée ou que la zone existe.");
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
                ModelState.AddModelError("", "Une place avec le même code existe déjà dans la zone sélectionnée.");
                return View(Place);
            }

            return RedirectToAction("Details", "Area", new { id = Place!.AreaId });
        }
    }
}