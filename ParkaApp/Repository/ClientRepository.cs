using ListeEtudiant.Data;
using Microsoft.EntityFrameworkCore;
using ParkaApp.Models;
using ParkaApp.Repository.Interfaces;

namespace ParkaApp.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllAsync(string? search = null)
        {
            var query = _context.Clients.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name!.ToLower().Contains(search.ToLower()) || c.CarPlate!.ToLower().Contains(search.ToLower()));
            }
        
            return await query.ToListAsync();
        }


        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _context.Clients.Include(c => c.Payments).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client?> GetByCarPlateAsync(string carPlate)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.CarPlate == carPlate);
        }

        public async Task<bool> AddAsync(Client Client)
        {
            // Check if a client with the same car plate already exists
            var existingClient = await GetByCarPlateAsync(Client.CarPlate);
            if (existingClient != null)
            {
                return false;
            }
            
            _context.Clients.Add(Client);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(Client Client)
        {
            // Check if a client with the same car plate already exists (excluding the current client)
            var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.CarPlate == Client.CarPlate && c.Id != Client.Id);
            if (existingClient != null)
            {
                return false;
            }

            _context.Clients.Update(Client);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Client = await GetByIdAsync(id);
            if (Client != null)
            {
                _context.Clients.Remove(Client);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}