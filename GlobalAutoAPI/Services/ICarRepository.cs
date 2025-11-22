using GlobalAutoLibrary.Models;

namespace GlobalAutoAPI.Services
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetCarsAsync(bool includeDetails);
        Task<IEnumerable<Car>> GetCarsByModelAsync(string model, bool includeDetails);
        Task<Car?> GetCarByIdAsync(int carId, bool includeDetails);
        Task AddCarAsync(Car car);
        void DeleteCar(Car car);
        Task<bool> CarExistsAsync(int carId);
        Task<bool> SaveAsync();
    }
}