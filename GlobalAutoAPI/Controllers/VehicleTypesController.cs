using AutoMapper;
using GlobalAutoAPI.DTO;
using GlobalAutoAPI.Services;
using GlobalAutoLibrary.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAutoAPI.Controllers
{
    //making specific route "api/types"
    [ApiController]
    [Route("api/types")] 
    public class VehicleTypesController : ControllerBase
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        private readonly IMapper _mapper;

        public VehicleTypesController(IVehicleTypeRepository vehicleTypeRepository, IMapper mapper)
        {
            _vehicleTypeRepository = vehicleTypeRepository ?? throw new ArgumentNullException(nameof(vehicleTypeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET (GetAll): api/types
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleTypeWithoutCarsDto>>> GetVehicleTypes()
        {
            var vehicleTypeEntities = await _vehicleTypeRepository.GetVehicleTypesAsync(includeCars: false);
            return Ok(_mapper.Map<IEnumerable<VehicleTypeWithoutCarsDto>>(vehicleTypeEntities));
        }

        // GET (GetById): api/types/{vehicleTypeId}
        [HttpGet("{vehicleTypeId}", Name = "GetVehicleType")]
        public async Task<IActionResult> GetVehicleType(int vehicleTypeId, bool includeCars = false)
        {
            var vehicleType = await _vehicleTypeRepository.GetVehicleTypeByIdAsync(vehicleTypeId, includeCars);

            if (vehicleType == null) return NotFound();

            if (includeCars)
            {
                return Ok(_mapper.Map<VehicleTypeDto>(vehicleType));
            }

            return Ok(_mapper.Map<VehicleTypeWithoutCarsDto>(vehicleType));
        }

        // POST: api/types
        [HttpPost]
        public async Task<ActionResult<VehicleTypeDto>> CreateVehicleType(
            VehicleTypeForManipulationDto vehicleTypeForCreation)
        {
            var vehicleTypeEntity = _mapper.Map<VehicleType>(vehicleTypeForCreation);

     
            await _vehicleTypeRepository.AddVehicleTypeAsync(vehicleTypeEntity);
            await _vehicleTypeRepository.SaveAsync();

            var vehicleTypeToReturn = _mapper.Map<VehicleTypeDto>(vehicleTypeEntity);

            return CreatedAtRoute("GetVehicleType",new { vehicleTypeId = vehicleTypeToReturn.VehicleTypeId },vehicleTypeToReturn);
        }

        // PUT (Full Update): api/types/{vehicleTypeId}
        [HttpPut("{vehicleTypeId}")]
        public async Task<ActionResult> UpdateVehicleType(int vehicleTypeId,
            VehicleTypeForManipulationDto vehicleTypeForUpdate)
        {
            var vehicleTypeEntity = await _vehicleTypeRepository.GetVehicleTypeByIdAsync(vehicleTypeId, includeCars: false);

            if (vehicleTypeEntity == null) return NotFound();

            _mapper.Map(vehicleTypeForUpdate, vehicleTypeEntity);
            await _vehicleTypeRepository.SaveAsync();

            return NoContent();
        }

        // PATCH (Partial Update): api/types/{vehicleTypeId}
        [HttpPatch("{vehicleTypeId}")]
        public async Task<ActionResult> PartiallyUpdateVehicleType(
            int vehicleTypeId,
            [FromBody] JsonPatchDocument<VehicleTypeForManipulationDto> patchDocument)
        {
            var vehicleTypeEntity = await _vehicleTypeRepository.GetVehicleTypeByIdAsync(vehicleTypeId, includeCars: false);

            if (vehicleTypeEntity == null) return NotFound();

            var vehicleTypeToPatch = _mapper.Map<VehicleTypeForManipulationDto>(vehicleTypeEntity);
            patchDocument.ApplyTo(vehicleTypeToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!TryValidateModel(vehicleTypeToPatch)) return BadRequest(ModelState);

            _mapper.Map(vehicleTypeToPatch, vehicleTypeEntity);

            await _vehicleTypeRepository.SaveAsync();

            return NoContent();
        }

        // DELETE: api/types/{vehicleTypeId}
        [HttpDelete("{vehicleTypeId}")]
        public async Task<ActionResult> DeleteVehicleType(int vehicleTypeId)
        {
            var vehicleTypeEntity = await _vehicleTypeRepository.GetVehicleTypeByIdAsync(vehicleTypeId, includeCars: false);

            if (vehicleTypeEntity == null) return NotFound();

            _vehicleTypeRepository.DeleteVehicleType(vehicleTypeEntity);
            await _vehicleTypeRepository.SaveAsync();

            return NoContent();
        }
    }
}