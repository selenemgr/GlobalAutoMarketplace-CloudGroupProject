using AutoMapper;
using GlobalAutoAPI.DTO;
using GlobalAutoAPI.Services;
using GlobalAutoLibrary.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAutoAPI.Controllers
{
    //making specific route "api/brands"
    [ApiController]
    [Route("api/brands")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandsController(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }


        // GET (GetAll): api/brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandWithoutCarsDto>>> GetBrands()
        {
            var brandEntities = await _brandRepository.GetBrandsAsync(includeCars: false);
            return Ok(_mapper.Map<IEnumerable<BrandWithoutCarsDto>>(brandEntities));
        }

        // GET (GetById): api/brands/{brandId}
        [HttpGet("{brandId}", Name = "GetBrand")]
        public async Task<IActionResult> GetBrand(int brandId, bool includeCars = false)
        {
            var brand = await _brandRepository.GetBrandByIdAsync(brandId, includeCars);

            if (brand == null) return NotFound();

            if (includeCars)
            {
                return Ok(_mapper.Map<BrandDto>(brand));
            }

            return Ok(_mapper.Map<BrandWithoutCarsDto>(brand));
        }

        // POST: api/brands
        [HttpPost]
        public async Task<ActionResult<BrandDto>> CreateBrand([FromBody] BrandForManipulationDto brandForCreation)
        {
            var brandEntity = _mapper.Map<Brand>(brandForCreation);

            await _brandRepository.AddBrandAsync(brandEntity);
            await _brandRepository.SaveAsync();

            var createdBrandToReturn = _mapper.Map<BrandDto>(brandEntity);

            return CreatedAtRoute("GetBrand", new { brandId = createdBrandToReturn.BrandId }, createdBrandToReturn);
        }

        // PUT (Replace): api/brands/{brandId}
        [HttpPut("{brandId}")]
        public async Task<ActionResult> UpdateBrand(int brandId, [FromBody] BrandForManipulationDto brandForUpdate)
        {
            var brandEntity = await _brandRepository.GetBrandByIdAsync(brandId, includeCars: false);

            if (brandEntity == null) return NotFound();

            _mapper.Map(brandForUpdate, brandEntity);
            await _brandRepository.SaveAsync();

            return NoContent(); 
        }

        // PATCH (Partial Update): api/brands/{brandId}
        [HttpPatch("{brandId}")]
        public async Task<ActionResult> PartiallyUpdateBrand(
            int brandId,
            [FromBody] JsonPatchDocument<BrandForManipulationDto> patchDocument)
        {
            var brandEntity = await _brandRepository.GetBrandByIdAsync(brandId, includeCars: false);

            if (brandEntity == null) return NotFound();

            var brandToPatch = _mapper.Map<BrandForManipulationDto>(brandEntity);
            patchDocument.ApplyTo(brandToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!TryValidateModel(brandToPatch)) return BadRequest(ModelState);

            _mapper.Map(brandToPatch, brandEntity);

            await _brandRepository.SaveAsync();

            return NoContent();
        }

        // DELETE: api/brands/{brandId}
        [HttpDelete("{brandId}")]
        public async Task<ActionResult> DeleteBrand(int brandId)
        {
            var brandEntity = await _brandRepository.GetBrandByIdAsync(brandId, includeCars: false);

            if (brandEntity == null) return NotFound();

            _brandRepository.DeleteBrand(brandEntity);
            await _brandRepository.SaveAsync();

            return NoContent();
        }
    }
}