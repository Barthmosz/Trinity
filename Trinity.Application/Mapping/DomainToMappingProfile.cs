using System.Diagnostics.CodeAnalysis;
using Trinity.Application.DTOs.Account;
using Trinity.Application.DTOs.Product;
using Trinity.Domain.Entities;

namespace Trinity.Application.Mapping
{
    [ExcludeFromCodeCoverage]
    public class DomainToMappingProfile : AutoMapper.Profile
    {
        public DomainToMappingProfile()
        {
            CreateMap<Product, ProductAddInput>().ReverseMap();
            CreateMap<Product, ProductOutput>().ReverseMap();

            CreateMap<Account, AccountSignUpInput>().ReverseMap();
            CreateMap<Account, AccountOutput>().ReverseMap();
        }
    }
}
