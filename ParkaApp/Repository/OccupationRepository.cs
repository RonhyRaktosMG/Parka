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
            return await _context.Occupations.Include(p => p.Client).Include(p => p.Place).ToListAsync();
        }

        public async Task<Occupation?> GetByIdAsync(int id)
        {
            return await _context.Occupations.Include(p => p.Client).Include(p => p.Place).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Occupation Occupation)
        {
            _context.Occupations.Add(Occupation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Occupation Occupation)
        {
            _context.Occupations.Update(Occupation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Occupation = await GetByIdAsync(id);
            if (Occupation != null)
            {
                _context.Occupations.Remove(Occupation);
                await _context.SaveChangesAsync();
            }
        }
    }
}