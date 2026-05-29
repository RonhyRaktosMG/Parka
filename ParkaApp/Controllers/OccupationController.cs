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
        private readonly IPlaceRepository _placeRepository;
        
        
        public OccupationController(IOccupationRepository repository, IClientRepository clientRepository, IPlaceRepository placeRepository, IPaymentRepository paymentRepository)
        {
            _repository = repository;
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
                bool result = await _repository.AddAsync(Occupation);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la création de l'occupation.");
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Occupation Occupation)
        {
            if (ModelState.IsValid)
            {
                bool result = await _repository.UpdateAsync(Occupation);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la mise à jour de l'occupation.");
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
                ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la suppression de l'occupation.");
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
                ModelState.AddModelError("PlaceId", "La place sélectionnée est invalide.");
                return NotFound();
            }

            bool result = await _repository.AssignPlaceAsync(vm.PlaceId, vm.CarPlate);
            if (!result)            
            {
                ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de l'affectation de la place. Veuillez vous assurer que la place est disponible.");
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

            // Check place
            if (occupation.Place == null)
            {
                ModelState.AddModelError(string.Empty, "La place occupée n'a pas pu être trouvée.");
                return View();
            }

            var vm = new ReleasePlaceViewModel
            {
                OccupationId = occupation.Id,
                PlaceId = occupation.PlaceId,
                CarPlate = occupation.Client != null ? occupation.Client.CarPlate : "--",
                IsClientGuest = occupation.Client != null && occupation.Client.IsGuest,
                EntryTime = occupation.EntryTime,
                ExitTime = DateTime.Now,
                TotalCost = _repository.CalculateTotalAmount(occupation.EntryTime, DateTime.Now),
                TotalToPay = (occupation.Client != null && occupation.Client.IsGuest) ? _repository.CalculateTotalAmount(occupation.EntryTime, DateTime.Now) : 0       
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
                ModelState.AddModelError(string.Empty, "Vérifiez que la place est actuellement occupée et que l'abonnement du client est valide s'il s'agit d'un abonné.");
                return View("ReleasePlace", new ReleasePlaceViewModel
                {                    
                    OccupationId = occupation.Id,
                    PlaceId = occupation.PlaceId,
                    CarPlate = occupation.Client != null ? occupation.Client.CarPlate : "--",
                    EntryTime = occupation.EntryTime,
                    ExitTime = DateTime.Now,
                    TotalCost = _repository.CalculateTotalAmount(occupation.EntryTime, DateTime.Now)
                });
            }

            return RedirectToAction("Details", "Area", new { id = occupation!.Place!.AreaId });
        }
 
 
 
 
        // Statistic
        public async Task<IActionResult> Statistics( DateTime? startDate, DateTime? endDate, string? areaName)
        {
            // Debug
            Console.WriteLine($"\n\n\n\n\nReceived dates: StartDate={startDate}, EndDate={endDate}\n\n\n\n\n");


            DateTime actualStartDate = startDate ?? DateTime.Now.AddMonths(-1); // Last Month
            DateTime actualEndDate = endDate ?? DateTime.Now.AddDays(1); // Include today
            string actualAreaName = areaName ?? "All";

            Dictionary<string, int> areaStatistics = await _repository.GetAreaStatisticsAsync(actualStartDate, actualEndDate, actualAreaName);
            


            // Place statistics per area
            Dictionary<string, Dictionary<string, int>> placeStatisticsPerArea = await _repository.GetPlaceStatisticsPerAreaAsync(actualStartDate, actualEndDate, actualAreaName);


            // Liste select options 
            List<SelectListItem> areaOptions = placeStatisticsPerArea.Keys.Select(
                area => new SelectListItem {
                    Value = area,
                    Text = area
                }).ToList();
            areaOptions.Insert(0, new SelectListItem { Value = "All", Text = "Toutes les zones" });


            var vm = new StatisticViewModel
            {
                AreaStatistics = areaStatistics,
                PlaceStatisticsPerArea = placeStatisticsPerArea,
                StartDate = actualStartDate,
                EndDate = actualEndDate,
                AreaName = actualAreaName,
                AreaOptions = areaOptions
            };

            return View(vm);
        }
    }
}