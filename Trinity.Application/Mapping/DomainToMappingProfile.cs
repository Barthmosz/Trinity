using Trinity.Application.DTOs.Products;
using Trinity.Application.DTOs.Users;
using Trinity.Domain.Accounts;
using Trinity.Domain.Products;

namespace Trinity.Application.Mapping
{
    public class DomainToMappingProfile : AutoMapper.Profile
    {
        public DomainToMappingProfile()
        {
            CreateMap<Products, ProductsInput>().ReverseMap();
            CreateMap<Products, ProductsOutput>().ReverseMap();

            CreateMap<Accounts, AccountsInput>().ReverseMap();
            CreateMap<Accounts, AccountsOutput>().ReverseMap();
        }
    }
}
