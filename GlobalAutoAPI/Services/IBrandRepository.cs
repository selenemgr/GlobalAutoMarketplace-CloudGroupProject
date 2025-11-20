using GlobalAutoLibrary.Models;

namespace GlobalAutoAPI.Services
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetBrandsAsync(bool includeCars);
        Task<Brand?> GetBrandByIdAsync(int brandId, bool includeCars);
        Task<Brand?> GetBrandByNameAsync(string brandName, bool includeCars);
        Task AddBrandAsync(Brand brand);
        void DeleteBrand(Brand brand);
        Task<bool> BrandExistsAsync(int brandId);
        Task<bool> SaveAsync();
    }
}