using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;
using ParkaApp.ViewModels.Occupation;



namespace ParkaApp.Controllers
{
    public class OccupationController : Controller
    {
        private readonly IOccupationRepository _repository;
        private readonly IClientRepository _clientRepository;
        private readonly IPlaceRepository _placeRepository;
        private readonly IPaymentRepository _paymentRepository;
        
        
        public OccupationController(IOccupationRepository repository, IClientRepository clientRepository, IPlaceRepository placeRepository, IPaymentRepository paymentRepository)
        {
            _repository = repository;
            _clientRepository = clientRepository;
            _placeRepository = placeRepository;
            _paymentRepository = paymentRepository;
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
                bool result = await _repository.AddAsync(Occupation);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the occupation.");
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
                bool result = await _repository.UpdateAsync(Occupation);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "An error occurred while updating the occupation.");
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
            bool result = await _repository.DeleteAsync(id);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the occupation.");
                return View();
            }
            return RedirectToAction(nameof(Index));
        }


        // Assign Place
        public async Task<IActionResult> AssignPlace(int placeId)
        {
            var vm = new AssignPlaceViewModel
            {
                PlaceId = placeId,
                EntryTime = DateTime.Now
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AssignPlace(AssignPlaceViewModel vm)
        {            
            // For the view
            Place? place = await _placeRepository.GetByIdAsync(vm.PlaceId);
            if (place == null)
            {
                ModelState.AddModelError("PlaceId", "Invalid place selected.");
                return NotFound();
            }

            bool result = await _repository.AssignPlaceAsync(vm.PlaceId, vm.CarPlate);
            if (!result)            
            {
                ModelState.AddModelError(string.Empty, "An error occurred while assigning the place. Please make sure the place is available.");
                return View(vm);
            }

            return RedirectToAction("Details", "Area", new { id = place.AreaId });
        }
    
    

        // Release Place
        public async Task<IActionResult> ReleasePlace(int id)
        {
            var occupation = await _repository.GetByIdAsync(id);
            if (occupation == null)
            {
                return NotFound();
            }

            var place = await _placeRepository.GetByIdAsync(occupation.PlaceId);
            if (place == null)
            {
                return NotFound();
            }

            var vm = new ReleasePlaceViewModel
            {
                OccupationId = occupation.Id,
                PlaceId = place.Id,
                CarPlate = (await _clientRepository.GetByIdAsync(occupation.ClientId))?.CarPlate ?? "Unknown",
                EntryTime = occupation.EntryTime,
                ExitTime = DateTime.Now
            };

            return View(vm);
        }


        [HttpPost, ActionName("ReleasePlace")]
        public async Task<IActionResult> ReleasePlaceConfirmed(int id)
        {
            Occupation? occupation = await _repository.GetByIdAsync(id);
            if (occupation == null)
            {
                return NotFound();
            }
            

            bool result = await _repository.ReleasePlaceAsync(occupation.PlaceId);
            if (!result)           
            {
                ModelState.AddModelError(string.Empty, "Make sure the place is currently occupied and the client's subscription is valid if they are a subscriber.");
                return View("ReleasePlace", new ReleasePlaceViewModel
                {                    
                    OccupationId = occupation.Id,
                    PlaceId = occupation.PlaceId,
                    CarPlate = occupation.Client != null ? occupation.Client.CarPlate : "",
                    EntryTime = occupation.EntryTime,
                    ExitTime = DateTime.Now
                });
            }

            return RedirectToAction("Details", "Area", new { id = occupation!.Place!.AreaId });
        }
    }
}