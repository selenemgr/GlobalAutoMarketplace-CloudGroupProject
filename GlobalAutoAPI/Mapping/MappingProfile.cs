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
            // Map for list view AutoMapper for brands and the second one for type since it is a obj 
            CreateMap<Car, CarWithoutDetailsDto>().ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Bname)).ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.VehicleType.TypeName));
            //Added ReverseMap() for patch functionality to work instead of adding other mapping 
            CreateMap<CarForManipulationDto, Car>().ReverseMap();

            // brand mappings
            CreateMap<Brand, BrandDto>();
            CreateMap<Brand, BrandWithoutCarsDto>();
            //Added ReverseMap() for patch functionality to work instead of adding other mapping 
            CreateMap<BrandForManipulationDto, Brand>().ReverseMap();  

            // type mappings
            CreateMap<VehicleType, VehicleTypeDto>();
            CreateMap<VehicleType, VehicleTypeWithoutCarsDto>();
            //Added ReverseMap() for patch functionality to work instead of adding other mapping 
            CreateMap<VehicleTypeForManipulationDto, VehicleType>().ReverseMap();
        }
    }
}