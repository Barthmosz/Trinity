using AutoMapper;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Account;
using Trinity.Application.Exceptions;
using Trinity.Application.Wrappers;
using Trinity.Domain.Entities;
using Trinity.Persistence.Contracts;

namespace Trinity.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IStaticPersistence<Account> AccountStaticPersistence;
        private readonly IDynamicPersistence<Account> AccountBasePersistence;
        private readonly IPasswordHasherWrapper PasswordHasherWrapper;
        private readonly ITokenService TokenService;
        private readonly IMapper Mapper;

        public AccountService(
            IStaticPersistence<Account> accountStaticPersistence,
            IDynamicPersistence<Account> accountBasePersistence,
            IPasswordHasherWrapper passwordHasherWrapper,
            ITokenService tokenService,
            IMapper mapper)
        {
            AccountStaticPersistence = accountStaticPersistence;
            AccountBasePersistence = accountBasePersistence;
            PasswordHasherWrapper = passwordHasherWrapper;
            TokenService = tokenService;
            Mapper = mapper;
        }

        public async Task<AccountOutput> SignUpAsync(AccountSignUpInput accountSignUpInput)
        {
            Account? accountExists = await AccountStaticPersistence.GetByEmailAsync(accountSignUpInput.Email);

            if (accountExists != null)
            {
                throw new AccountException("Email already registered.");
            }

            Account account = Mapper.Map<Account>(accountSignUpInput);
            account.PasswordHash = PasswordHasherWrapper.Hash(accountSignUpInput.Password);

            AccountOutput accountOutput = Mapper.Map<AccountOutput>(account);

            await AccountBasePersistence.AddAsync(account);
            return accountOutput;
        }

        public async Task<TokenOutput> SignInAsync(AccountSignInInput accountSignInInput)
        {
            Account? account = await AccountStaticPersistence.GetByEmailAsync(accountSignInInput.Email) ?? throw new AccountException("Email not registered.");

            if (!PasswordHasherWrapper.Verify(account.PasswordHash, accountSignInInput.Password))
            {
                throw new AccountException("Invalid email or password.");
            }

            TokenOutput token = TokenService.GenerateToken(account);
            return token;
        }
    }
}
