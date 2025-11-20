using GlobalAutoLibrary.Models;

namespace GlobalAutoAPI.Services
{
    public interface IVehicleTypeRepository
    {
        Task<IEnumerable<VehicleType>> GetVehicleTypesAsync(bool includeCars);
        Task<VehicleType?> GetVehicleTypeByIdAsync(int vehicleTypeId, bool includeCars);
        Task<VehicleType?> GetVehicleTypeByNameAsync(string typeName, bool includeCars);
        Task AddVehicleTypeAsync(VehicleType vehicleType);
        void DeleteVehicleType(VehicleType vehicleType);
        Task<bool> VehicleTypeExistsAsync(int vehicleTypeId);
        Task<bool> SaveAsync();
    }
}