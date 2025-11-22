using GlobalAutoLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalAutoAPI.Services
{
    public class BrandRepository : IBrandRepository
    {
        private readonly GlobalAutoDBContext _context;

        public BrandRepository(GlobalAutoDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> BrandExistsAsync(int brandId)
        {
            return await _context.Brands.AnyAsync(b => b.BrandId == brandId);
        }

        public async Task<IEnumerable<Brand>> GetBrandsAsync(bool includeCars)
        {
            IQueryable<Brand> collection = _context.Brands;
            if (includeCars)
            {
                collection = collection.Include(b => b.Cars);
            }
            return await collection.OrderBy(b => b.Bname).ToListAsync();
        }

        public async Task<Brand?> GetBrandByIdAsync(int brandId, bool includeCars)
        {
            IQueryable<Brand> collection = _context.Brands;
            if (includeCars)
            {
                collection = collection.Include(b => b.Cars);
            }
            return await collection.FirstOrDefaultAsync(b => b.BrandId == brandId);
        }

        public async Task<Brand?> GetBrandByNameAsync(string brandName, bool includeCars)
        {
            IQueryable<Brand> collection = _context.Brands;
            if (includeCars)
            {
                collection = collection.Include(b => b.Cars);
            }
            return await collection.FirstOrDefaultAsync(b => b.Bname.ToLower() == brandName.ToLower());
        }

        public async Task AddBrandAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
        }

        public void DeleteBrand(Brand brand)
        {
            _context.Brands.Remove(brand);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}