using ParkaApp.Models;

namespace ParkaApp.Repository.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task AddAsync(Client Client);
        Task UpdateAsync(Client Client);
        Task DeleteAsync(int id);
    }
}