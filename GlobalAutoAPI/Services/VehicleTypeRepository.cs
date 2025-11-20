using GlobalAutoLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalAutoAPI.Services
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly GlobalAutoDBContext _context;

        public VehicleTypeRepository(GlobalAutoDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> VehicleTypeExistsAsync(int vehicleTypeId)
        {
            return await _context.VehicleTypes.AnyAsync(v => v.VehicleTypeId == vehicleTypeId);
        }

        public async Task<IEnumerable<VehicleType>> GetVehicleTypesAsync(bool includeCars)
        {
            IQueryable<VehicleType> collection = _context.VehicleTypes;

            if (includeCars)
            {
                collection = collection.Include(v => v.Cars);
            }

            return await collection.OrderBy(v => v.TypeName).ToListAsync();
        }

        public async Task<VehicleType?> GetVehicleTypeByIdAsync(int vehicleTypeId, bool includeCars)
        {
            IQueryable<VehicleType> collection = _context.VehicleTypes;

            if (includeCars)
            {
                collection = collection.Include(v => v.Cars);
            }

            return await collection.FirstOrDefaultAsync(v => v.VehicleTypeId == vehicleTypeId);
        }

        public async Task<VehicleType?> GetVehicleTypeByNameAsync(string typeName, bool includeCars)
        {
            IQueryable<VehicleType> collection = _context.VehicleTypes;

            if (includeCars)
            {
                collection = collection.Include(v => v.Cars);
            }
            return await collection.FirstOrDefaultAsync(v => v.TypeName.ToLower() == typeName.ToLower());
        }

        public async Task AddVehicleTypeAsync(VehicleType vehicleType)
        {
            await _context.VehicleTypes.AddAsync(vehicleType);
        }

        public void DeleteVehicleType(VehicleType vehicleType)
        {
            _context.VehicleTypes.Remove(vehicleType);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}