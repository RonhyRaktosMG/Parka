using ListeEtudiant.Data;
using Microsoft.EntityFrameworkCore;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;

namespace ParkaApp.Repository
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly AppDbContext _context;

        public PlaceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Place>> GetAllAsync()
        {
            return await _context.Places.Include(p => p.Area).ToListAsync();
        }

        public async Task<Place?> GetByIdAsync(int id)
        {
            return await _context.Places.Include(p => p.Area).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> AddAsync(Place Place)
        {
            // Check if the Area exists 
            var area = await _context.Areas.FindAsync(Place.AreaId);
            if (area == null) 
            {
                return false; 
            }
            
            // Check if the Place Code is unique within the Area
            bool isCodeUnique = !_context.Places.Any(p => p.AreaId == Place.AreaId && p.Code == Place.Code);
            if (!isCodeUnique)
            {
                return false;
            }

            _context.Places.Add(Place);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(Place Place)
        {
            // Check if the Place exists except the current one
            bool isCodeUnique = !_context.Places.Any(p => p.AreaId == Place.AreaId && p.Code == Place.Code && p.Id != Place.Id);
            if (!isCodeUnique)
            {                
                return false;
            }                                   

            // Check if the Area exists
            var area = await _context.Areas.FindAsync(Place.AreaId);
            if (area == null)
            {
                return false;
            }                

            _context.Places.Update(Place);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Place = await GetByIdAsync(id);
            if (Place != null)
            {
                _context.Places.Remove(Place);
                await _context.SaveChangesAsync();
                
                return true;
            }
            return false;
        }
    }
}