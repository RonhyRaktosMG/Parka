using ParkaApp.Models;

namespace ParkaApp.Repository.Interfaces
{
    public interface IPlaceRepository
    {
        Task<IEnumerable<Place>> GetAllAsync();
        Task<Place?> GetByIdAsync(int id);
        Task<bool> AddAsync(Place place);
        Task<bool> UpdateAsync(Place place);
        Task<bool> DeleteAsync(int id);
    }
}