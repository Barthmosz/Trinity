using Trinity.Application.DTOs.Products;
using Trinity.Application.DTOs.Users;
using Trinity.Domain;

namespace Trinity.Application.Mapping
{
    public class DomainToMappingProfile : AutoMapper.Profile
    {
        public DomainToMappingProfile()
        {
            CreateMap<Products, ProductsInput>().ReverseMap();
            CreateMap<Products, UsersOutput>().ReverseMap();

            CreateMap<Users, UsersInput>().ReverseMap();
            CreateMap<Users, UsersOutput>().ReverseMap();
        }
    }
}
