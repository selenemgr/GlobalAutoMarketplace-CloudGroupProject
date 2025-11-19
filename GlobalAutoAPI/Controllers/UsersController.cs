using AutoMapper;
using GlobalAutoAPI.DTO;
using GlobalAutoAPI.Services;
using GlobalAutoLibrary.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAutoAPI.Controllers
{
    //making specific route "api/users"
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET (GetAll): api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithoutEmailDto>>> GetUsers()
        {
            var userEntities = await _userRepository.GetUsersAsync();
            return Ok(_mapper.Map<IEnumerable<UserWithoutEmailDto>>(userEntities));
        }

        // GET (GetById): api/users/{userId}
        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            return Ok(_mapper.Map<UserDto>(user));
        }

        // POST api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) ||
                string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            if (user.Password != loginDto.Password)
                return Unauthorized("Invalid email or password.");

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }


        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserForManipulationDto userForCreation)
        {
            var userEntity = _mapper.Map<User>(userForCreation);

            await _userRepository.AddUserAsync(userEntity);
            await _userRepository.SaveAsync();

            var createdUserToReturn = _mapper.Map<UserDto>(userEntity);

            return CreatedAtRoute("GetUser",
                new { userId = createdUserToReturn.UserId },
                createdUserToReturn);
        }

        // PUT to Replace: api/users/{userId}
        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateUser(int userId, [FromBody] UserForManipulationDto userForUpdate)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId);

            if (userEntity == null) return NotFound();

            _mapper.Map(userForUpdate, userEntity);
            await _userRepository.SaveAsync();

            return NoContent(); 
        }

        // PATCH for Partially Update: api/users/{userId}
        [HttpPatch("{userId}")]
        public async Task<ActionResult> PartiallyUpdateUser(
            int userId,
            [FromBody] JsonPatchDocument<UserForManipulationDto> patchDocument)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId);

            if (userEntity == null) return NotFound();

            var userToPatch = _mapper.Map<UserForManipulationDto>(userEntity);
            patchDocument.ApplyTo(userToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!TryValidateModel(userToPatch)) return BadRequest(ModelState);

            _mapper.Map(userToPatch, userEntity);

            await _userRepository.SaveAsync();

            return NoContent();
        }

        // DELETE: api/users/{userId}
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var userEntity = await _userRepository.GetUserByIdAsync(userId);

            if (userEntity == null) return NotFound();

            _userRepository.DeleteUser(userEntity);
            await _userRepository.SaveAsync();

            return NoContent();
        }
    }
}