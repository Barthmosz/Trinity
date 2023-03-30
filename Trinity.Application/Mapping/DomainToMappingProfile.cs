using Trinity.Application.DTOs.Products;
using Trinity.Domain;

namespace Trinity.Application.Mapping
{
    public class DomainToMappingProfile : AutoMapper.Profile
    {
        public DomainToMappingProfile()
        {
            CreateMap<Products, ProductsInput>().ReverseMap();
            CreateMap<Products, ProductsOutput>().ReverseMap();
        }
    }
}
