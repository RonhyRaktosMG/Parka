using ParkaApp.Models;

namespace ParkaApp.Repository.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllAsync(string? search = null);
        Task<Client?> GetByIdAsync(int id);
        Task<Client?> GetByCarPlateAsync(string carPlate);
        Task<bool> AddAsync(Client Client);
        Task<bool> UpdateAsync(Client Client);
        Task<bool> DeleteAsync(int id);
    }
}