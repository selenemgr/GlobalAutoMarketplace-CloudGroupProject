using AutoMapper;
using GlobalAutoAPI.DTO;
using GlobalAutoAPI.Services;
using GlobalAutoLibrary.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAutoAPI.Controllers
{
    //making specific route "api/cars"
    [ApiController]
    [Route("api/cars")]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        private readonly IMapper _mapper;

        public CarsController(
            ICarRepository carRepository,
            IBrandRepository brandRepository,
            IVehicleTypeRepository vehicleTypeRepository,
            IMapper mapper)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _brandRepository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
            _vehicleTypeRepository = vehicleTypeRepository ?? throw new ArgumentNullException(nameof(vehicleTypeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET (GetAll): api/cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetCars()
        {
            var carEntities = await _carRepository.GetCarsAsync(includeDetails: true);
            return Ok(_mapper.Map<IEnumerable<CarDto>>(carEntities));
        }

        // GET (GetById): api/cars/{carId}
        [HttpGet("{carId}", Name = "GetCar")]
        public async Task<IActionResult> GetCar(int carId, bool includeDetails = false)
        {
            var car = await _carRepository.GetCarByIdAsync(carId, includeDetails);

            if (car == null) return NotFound();

            if (includeDetails)
            {
                return Ok(_mapper.Map<CarDto>(car));
            }
            return Ok(_mapper.Map<CarWithoutDetailsDto>(car));
        }

        // Get By Model: api/cars/bymodel/{model}
        [HttpGet("bymodel/{model}")]
        public async Task<ActionResult<IEnumerable<CarWithoutDetailsDto>>> GetCarsByModel(string model, bool includeDetails = false)
        {
            var carEntities = await _carRepository.GetCarsByModelAsync(model, includeDetails);

            // If no match, return 404
            if (!carEntities.Any()) return NotFound();

            if (includeDetails)
            {
                // Return CarDto
                return Ok(_mapper.Map<IEnumerable<CarDto>>(carEntities));
            }

            //CarWithoutDetailsDto
            return Ok(_mapper.Map<IEnumerable<CarWithoutDetailsDto>>(carEntities));
        }

        // POST: api/cars
        [HttpPost]
        public async Task<ActionResult<CarDto>> CreateCar(CarForManipulationDto carForCreation)
        {
            // checking for brand Id exists
            if (!await _brandRepository.BrandExistsAsync(carForCreation.BrandId))
            {
                return NotFound($"Brand with id {carForCreation.BrandId} not found.");
            }

            // check if  Vehicle Type Id exists 
            if (!await _vehicleTypeRepository.VehicleTypeExistsAsync(carForCreation.VehicleTypeId))
            {
                return NotFound($"Vehicle Type with id {carForCreation.VehicleTypeId} not found.");
            }

            var carEntity = _mapper.Map<Car>(carForCreation);

            await _carRepository.AddCarAsync(carEntity);
            await _carRepository.SaveAsync();

            var createdCar = await _carRepository.GetCarByIdAsync(carEntity.CarId, includeDetails: true);

            var carToReturn = _mapper.Map<CarDto>(createdCar);
            return CreatedAtRoute("GetCar", new { carId = carToReturn.CarId }, carToReturn);
        }

        // PUT (Full Update): api/cars/{carId}
        [HttpPut("{carId}")]
        public async Task<ActionResult> UpdateCar(int carId,
            CarForManipulationDto carForUpdate)
        {
            var carEntity = await _carRepository.GetCarByIdAsync(carId, includeDetails: false);

            if (carEntity == null) return NotFound();

            //  check for BrandId
            if (!await _brandRepository.BrandExistsAsync(carForUpdate.BrandId))
            {
                return NotFound($"Brand with id {carForUpdate.BrandId} not found.");
            }

            // check for VehicleTypeId
            if (!await _vehicleTypeRepository.VehicleTypeExistsAsync(carForUpdate.VehicleTypeId))
            {
                return NotFound($"Vehicle Type with id {carForUpdate.VehicleTypeId} not found.");
            }

            _mapper.Map(carForUpdate, carEntity);
            await _carRepository.SaveAsync();

            return NoContent();
        }

        // PATCH it will partially Update: api/cars/{carId}
        //here where we can use the Manipulation DTO and this goes for the three controllers
        [HttpPatch("{carId}")]
        public async Task<ActionResult> PartiallyUpdateCar(
            int carId,
            [FromBody] JsonPatchDocument<CarForManipulationDto> patchDocument)
        {
            var carEntity = await _carRepository.GetCarByIdAsync(carId, includeDetails: false);

            if (carEntity == null) return NotFound();

            var carToPatch = _mapper.Map<CarForManipulationDto>(carEntity);

            patchDocument.ApplyTo(carToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!TryValidateModel(carToPatch)) return BadRequest(ModelState);

            // Check for the keys after patching the DTO
            if (!await _brandRepository.BrandExistsAsync(carToPatch.BrandId))
            {
                return NotFound($"Brand with id {carToPatch.BrandId} not found.");
            }

            if (!await _vehicleTypeRepository.VehicleTypeExistsAsync(carToPatch.VehicleTypeId))
            {
                return NotFound($"Vehicle Type with id {carToPatch.VehicleTypeId} not found.");
            }

            _mapper.Map(carToPatch, carEntity);
            await _carRepository.SaveAsync();

            return NoContent();
        }

        // DELETE: api/cars/{carId}
        [HttpDelete("{carId}")]
        public async Task<ActionResult> DeleteCar(int carId)
        {
            var carEntity = await _carRepository.GetCarByIdAsync(carId, includeDetails: false);

            if (carEntity == null) return NotFound();

            _carRepository.DeleteCar(carEntity);
            await _carRepository.SaveAsync();

            return NoContent();
        }
    }
}