using System.Diagnostics.CodeAnalysis;
using Trinity.Application.DTOs.Products;
using Trinity.Application.DTOs.Users;
using Trinity.Domain.Entities.Accounts;
using Trinity.Domain.Entities.Products;

namespace Trinity.Application.Mapping
{
    [ExcludeFromCodeCoverage]
    public class DomainToMappingProfile : AutoMapper.Profile
    {
        public DomainToMappingProfile()
        {
            CreateMap<Products, ProductsAddInput>().ReverseMap();
            CreateMap<Products, ProductsOutput>().ReverseMap();

            CreateMap<Accounts, AccountsSignUpInput>().ReverseMap();
            CreateMap<Accounts, AccountsOutput>().ReverseMap();
        }
    }
}
