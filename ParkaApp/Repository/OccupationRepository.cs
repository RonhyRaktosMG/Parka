using ListeEtudiant.Data;
using Microsoft.EntityFrameworkCore;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;

namespace ParkaApp.Repository
{
    public class OccupationRepository : IOccupationRepository
    {
        private readonly AppDbContext _context;

        public OccupationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Occupation>> GetAllAsync()
        {
            return await _context.Occupations.Include(p => p.Client).Include(p => p.Place).Include(p => p.Place!.Area).ToListAsync();
        }

        public async Task<Occupation?> GetByIdAsync(int id)
        {
            return await _context.Occupations.Include(p => p.Client).Include(p => p.Place).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> AddAsync(Occupation Occupation)
        {
            // Check if the occupation already exists (by id)
            var exists = await _context.Occupations.AnyAsync(o => o.Id == Occupation.Id);
            if (exists)
            {
                return false;
            }

            // Check if the place exists
            var place = await _context.Places.FindAsync(Occupation.PlaceId);
            if (place == null)            {
                return false;
            }

            // Check if the client exists
            var client = await _context.Clients.FindAsync(Occupation.ClientId);
            if (client == null)            {
                return false;
            }

            await _context.Occupations.AddAsync(Occupation);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> UpdateAsync(Occupation Occupation)
        {
            // Check if the occupation exists
            var existingOccupation = await _context.Occupations.AsNoTracking().FirstOrDefaultAsync(o => o.Id == Occupation.Id);
            if (existingOccupation == null)
            {
                return false;
            }

            // Check if the place exists
            var place = await _context.Places.FindAsync(Occupation.PlaceId);
            if (place == null)            
            {
                return false;
            }

            // Check if the client exists
            var client = await _context.Clients.FindAsync(Occupation.ClientId);
            if (client == null)           
            {
                return false;
            }

            _context.Occupations.Update(Occupation);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Occupation = await GetByIdAsync(id);
            if (Occupation != null)
            {
                // Change place status to available before deleting the occupation
                var place = await _context.Places.FindAsync(Occupation.PlaceId);
                if (place == null)
                {
                    return false; // Place not found
                }        

                place.Status = PlaceStatus.Available;
                _context.Places.Update(place);

                _context.Occupations.Remove(Occupation);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    
        public async Task<bool> AssignPlaceAsync(int placeId, string carPlate)
        {
            if (string.IsNullOrEmpty(carPlate))
            {
                return false;
            }

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.CarPlate == carPlate);
            var place = await _context.Places.FindAsync(placeId);

             // If place doesn't exist, return error
            if (place == null)
            {   
                return false;
            }

            // If place is already occupied, return error
            if (place.Status == PlaceStatus.Occupied || place.Status == PlaceStatus.Reserved)
            {
                return false;
            }

            // If client doesn't exist, create a new one
            if (client == null)
            {
                client = new Client
                {
                    Name = "Guest" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    CarPlate = carPlate,
                    IsGuest = true
                };

                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();
            }
            client = await _context.Clients.FirstOrDefaultAsync(c => c.CarPlate == carPlate);

            // If client still can't be found, there is an error
            if (client == null)
            {
                return false;
            }


            // Create new occupation
            var occupation = new Occupation
            {
                PlaceId = placeId,
                ClientId = client.Id,
                EntryTime = DateTime.Now,
            };

            // Change Place status to occupied
            place.Status = PlaceStatus.Occupied;
            await _context.SaveChangesAsync();

            await _context.Occupations.AddAsync(occupation);
            await _context.SaveChangesAsync();

            return true;
        }
    
        public async Task<bool> ReleasePlaceAsync(int placeId)
        {
            Occupation? occupation = await _context.Occupations.FirstOrDefaultAsync(o => o.PlaceId == placeId && o.ExitTime == null);
            if (occupation == null)
            {
                return false;
            }

            Place? place = await _context.Places.FindAsync(occupation.PlaceId);
            if (place == null)
            {
                return false;
            }


            Client? client = await _context.Clients.FindAsync(occupation.ClientId);
            if (client == null)
            {
                return false;
            }

            // Make payment if client is a guest
            if (client.IsGuest)
            {
                // Payment
                var payment = new Payment
                {
                    ClientId = client.Id,
                    Amount = CalculateTotalAmount(occupation.EntryTime, occupation.ExitTime ?? DateTime.Now),
                    Type = PaymentType.Hourly, 
                    StartDate = occupation.EntryTime,
                    EndDate = occupation.ExitTime ?? DateTime.Now,
                };

                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
            }

            // Else verify subscription if client is a subscriber
            else
            {
                // Get the last subscription of the client
                Payment? subscription = await _context.Payments.Where(p => p.ClientId == client.Id)
                    .OrderByDescending(p => p.EndDate)
                    .FirstOrDefaultAsync();

                if (subscription == null || subscription.EndDate < DateTime.Now)
                {
                    // Subscription has expired
                    return false;
                }
            }

            // Change Place status to available
            place.Status = PlaceStatus.Available;
            await _context.SaveChangesAsync();

            // Set exit time for the occupation
            occupation.ExitTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }

        public double CalculateTotalAmount(DateTime entryTime, DateTime exitTime)
        {   
            var HOURLY_PRICE = 500;
        
            var duration = exitTime - entryTime;
            return Math.Ceiling(duration.TotalHours) * HOURLY_PRICE;

        }


        // Statistics
        public async Task<Dictionary<string, int>> GetAreaStatisticsAsync(DateTime startDate, DateTime endDate, string areaName = "All")
        {
            return await _context.Occupations
                .Include(o => o.Place)
                    .ThenInclude(p => p!.Area)
                .Where(o => o.Place != null && o.Place.Area != null && (areaName == "All" || o.Place.Area.Name == areaName) && o.EntryTime >= startDate && (o.ExitTime == null || o.ExitTime <= endDate))
                .GroupBy(o => o.Place!.Area!.Name)
                .Select(g => new { Area = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Area, x => x.Count);
        }

        public async Task<Dictionary<string, Dictionary<string, int>>> GetPlaceStatisticsPerAreaAsync(DateTime startDate, DateTime endDate, string areaName = "All")
        {
            var areas = await _context.Areas.ToListAsync();

            var result = new Dictionary<string, Dictionary<string, int>>();

            foreach (var area in areas)
            {
                var placeStats = await _context.Occupations
                .Include(o => o.Place)
                .Where(o => o.Place != null && o.Place.AreaId == area.Id && (areaName == "All" || o.Place!.Area!.Name == areaName) && o.EntryTime >= startDate && (o.ExitTime == null || o.ExitTime <= endDate))
                .GroupBy(o => o.Place!.Code)
                .Select(g => new { Place = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToDictionaryAsync(x => x.Place!, x => x.Count); 
                
                result[area.Name] = placeStats;
            }

            return result;
        }
    }
}