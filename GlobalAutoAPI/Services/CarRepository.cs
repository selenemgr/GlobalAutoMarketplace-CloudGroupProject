using GlobalAutoLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalAutoAPI.Services
{
    public class CarRepository : ICarRepository
    {
        private readonly GlobalAutoDBContext _context;

        public CarRepository(GlobalAutoDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CarExistsAsync(int carId)
        {
            return await _context.Cars.AnyAsync(c => c.CarId == carId);
        }

        public async Task<IEnumerable<Car>> GetCarsAsync(bool includeDetails)
        {
            IQueryable<Car> collection = _context.Cars;
            if (includeDetails)
            {
                
                collection = collection.Include(c => c.Brand).Include(c => c.VehicleType);
            }
            return await collection.OrderBy(c => c.Model).ToListAsync();
        }

        public async Task<Car?> GetCarByIdAsync(int carId, bool includeDetails)
        {
            IQueryable<Car> collection = _context.Cars;
            if (includeDetails)
            {
                collection = collection.Include(c => c.Brand).Include(c => c.VehicleType);
            }
            return await collection.FirstOrDefaultAsync(c => c.CarId == carId);
        }

        public async Task AddCarAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
        }

        public void DeleteCar(Car car)
        {
            _context.Cars.Remove(car);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}