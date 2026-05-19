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
        
        
        public OccupationController(IOccupationRepository repository, IClientRepository clientRepository, IPlaceRepository placeRepository)
        {
            _repository = repository;
            _clientRepository = clientRepository;
            _placeRepository = placeRepository;
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
            var client = await _clientRepository.GetByCarPlateAsync(vm.CarPlate);
            var place = await _placeRepository.GetByIdAsync(vm.PlaceId);
            
            if (place == null)
            {
                ModelState.AddModelError("PlaceId", "Invalid place selected.");
                return View(vm);
            }

            // If place is already occupied, return error
            if (place.Status == PlaceStatus.Occupied || place.Status == PlaceStatus.Reserved)
            {
                ModelState.AddModelError("PlaceId", "Selected place is already occupied or reserved.");
                return View(vm);
            }

            // If client doesn't exist, create a new one
            if (client == null)
            {
                client = new Client
                {
                    CarPlate = vm.CarPlate,
                    IsGuest = true
                };

                await _clientRepository.AddAsync(client);
            }
            client = await _clientRepository.GetByCarPlateAsync(vm.CarPlate);

            // If client still can't be found, there is an error
            if (client == null)
            {
                ModelState.AddModelError("CarPlate", "Unable to find or create client with the provided car plate.");
                return View(vm);
            }

            var occupation = new Occupation
            {
                PlaceId = vm.PlaceId,
                ClientId = client.Id,
                EntryTime = vm.EntryTime,
            };

            // Change Place status to occupied
            place.Status = PlaceStatus.Occupied;
            await _placeRepository.UpdateAsync(place);

            await _repository.AddAsync(occupation);

            return RedirectToAction("Index");
        }
    }
}