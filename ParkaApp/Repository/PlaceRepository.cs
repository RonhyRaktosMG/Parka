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

        public async Task AddAsync(Place Place)
        {
            _context.Places.Add(Place);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Place Place)
        {
            _context.Places.Update(Place);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Place = await GetByIdAsync(id);
            if (Place != null)
            {
                _context.Places.Remove(Place);
                await _context.SaveChangesAsync();
            }
        }
    }
}