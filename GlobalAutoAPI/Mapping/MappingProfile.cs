using AutoMapper;
using GlobalAutoLibrary.Models;
using GlobalAutoAPI.DTO;

namespace GlobalAutoAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // cars mappings
            CreateMap<Car, CarDto>();
            CreateMap<Car, CarWithoutDetailsDto>();
            CreateMap<CarForManipulationDto, Car>();

            // brand mappings
            CreateMap<Brand, BrandDto>();
            CreateMap<Brand, BrandWithoutCarsDto>();
            CreateMap<BrandForManipulationDto, Brand>();

            // users mapping
            CreateMap<User, UserDto>();
            CreateMap<User, UserWithoutEmailDto>();
            CreateMap<UserForManipulationDto, User>();
        }
    }
}
