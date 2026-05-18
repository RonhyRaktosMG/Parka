using ParkaApp.Models;

namespace ParkaApp.Repository.Interfaces
{
    public interface IAreaRepository
    {
        Task<IEnumerable<Area>> GetAllAsync();
        Task<Area?> GetAreaWithPlacesAsync(int id);
        Task<Area?> GetByIdAsync(int id);
        Task AddAsync(Area area);
        Task UpdateAsync(Area area);
        Task DeleteAsync(int id);
    }
}