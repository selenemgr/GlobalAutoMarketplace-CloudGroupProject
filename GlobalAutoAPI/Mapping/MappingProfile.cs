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
            CreateMap<Car, CarWithoutDetailsDto>().ForMember(dest => dest.BrandName,opt => opt.MapFrom(src => src.Brand.Bname));
            CreateMap<CarForManipulationDto, Car>();

            // brand mappings
            CreateMap<Brand, BrandDto>();
            CreateMap<Brand, BrandWithoutCarsDto>();
            CreateMap<BrandForManipulationDto, Brand>();

            // Vehicle type mappings
            CreateMap<VehicleType, VehicleTypeDto>();
            CreateMap<VehicleType, VehicleTypeWithoutCarsDto>();
            CreateMap<VehicleTypeForManipulationDto, VehicleType>();


        }
    }
}