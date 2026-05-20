using ParkaApp.Models;

namespace ParkaApp.Repository.Interfaces
{
    public interface IAreaRepository
    {
        Task<IEnumerable<Area>> GetAllAsync();
        Task<Area?> GetAreaWithPlacesAsync(int id);
        Task<Area?> GetByIdAsync(int id);
        Task<bool> AddAsync(Area area);
        Task<bool> UpdateAsync(Area area);
        Task<bool> DeleteAsync(int id);
    }
}