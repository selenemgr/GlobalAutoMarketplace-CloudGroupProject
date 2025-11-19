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
        private readonly IMapper _mapper;

        public CarsController(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
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

        // POST: api/cars
        [HttpPost]
        public async Task<ActionResult<CarDto>> CreateCar([FromBody] CarForManipulationDto carForCreation)
        {
            var carEntity = _mapper.Map<Car>(carForCreation);
            await _carRepository.AddCarAsync(carEntity);
            await _carRepository.SaveAsync();

            var createdCarToReturn = await _carRepository.GetCarByIdAsync(carEntity.CarId, includeDetails: true);

            return CreatedAtRoute("GetCar",
                new { carId = createdCarToReturn.CarId },
                _mapper.Map<CarDto>(createdCarToReturn));
        }

        // PUT it will Replace: api/cars/{carId}
        [HttpPut("{carId}")]
        public async Task<ActionResult> UpdateCar(int carId, [FromBody] CarForManipulationDto carForUpdate)
        {
            var carEntity = await _carRepository.GetCarByIdAsync(carId, includeDetails: false);

            if (carEntity == null) return NotFound();

            _mapper.Map(carForUpdate, carEntity);
            await _carRepository.SaveAsync();

            return NoContent(); // 204 No Content
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