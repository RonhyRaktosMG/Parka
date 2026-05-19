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

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }


        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<Client?> GetByCarPlateAsync(string carPlate)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.CarPlate == carPlate);
        }

        public async Task AddAsync(Client Client)
        {
            _context.Clients.Add(Client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client Client)
        {
            _context.Clients.Update(Client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Client = await GetByIdAsync(id);
            if (Client != null)
            {
                _context.Clients.Remove(Client);
                await _context.SaveChangesAsync();
            }
        }
    }
}