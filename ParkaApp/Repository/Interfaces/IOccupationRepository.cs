using ParkaApp.Models;

namespace ParkaApp.Repository.Interfaces
{
    public interface IOccupationRepository
    {
        Task<IEnumerable<Occupation>> GetAllAsync();
        Task<Occupation?> GetByIdAsync(int id);
        Task AddAsync(Occupation Occupation);
        Task UpdateAsync(Occupation Occupation);
        Task DeleteAsync(int id);
    }
}