using ListeEtudiant.Data;
using Microsoft.EntityFrameworkCore;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;

namespace ParkaApp.Repository
{
    public class AreaRepository : IAreaRepository
    {
        private readonly AppDbContext _context;

        public AreaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Area>> GetAllAsync()
        {
            return await _context.Areas.ToListAsync();
        }

        public async Task<Area?> GetAreaWithPlacesAsync(int id)
        {
            return await _context.Areas
                .Include(a => a.Places)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Area?> GetByIdAsync(int id)
        {
            return await _context.Areas.FindAsync(id);
        }

        public async Task AddAsync(Area area)
        {
            _context.Areas.Add(area);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Area area)
        {
            _context.Areas.Update(area);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var area = await GetByIdAsync(id);
            if (area != null)
            {
                _context.Areas.Remove(area);
                await _context.SaveChangesAsync();
            }
        }
    }
}