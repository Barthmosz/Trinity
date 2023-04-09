using AutoMapper;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Accounts;
using Trinity.Application.Exceptions.Accounts;
using Trinity.Application.Wrappers;
using Trinity.Domain.Entities.Accounts;
using Trinity.Persistence.Contracts;

namespace Trinity.Application.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IStaticPersistence<Accounts> AccountsStaticPersistence;
        private readonly IDynamicPersistence<Accounts> AccountsBasePersistence;
        private readonly IPasswordHasherWrapper PasswordHasherWrapper;
        private readonly ITokenService TokenService;
        private readonly IMapper Mapper;

        public AccountsService(
            IStaticPersistence<Accounts> accountsStaticPersistence,
            IDynamicPersistence<Accounts> accountsBasePersistence,
            IPasswordHasherWrapper passwordHasherWrapper,
            ITokenService tokenService,
            IMapper mapper)
        {
            AccountsStaticPersistence = accountsStaticPersistence;
            AccountsBasePersistence = accountsBasePersistence;
            PasswordHasherWrapper = passwordHasherWrapper;
            TokenService = tokenService;
            Mapper = mapper;
        }

        public async Task<AccountsOutput> SignUpAsync(AccountsSignUpInput accountInput)
        {
            Accounts? accountExists = await AccountsStaticPersistence.GetByEmailAsync(accountInput.Email);

            if (accountExists != null)
            {
                throw new AccountsException("Email already registered.");
            }

            Accounts account = Mapper.Map<Accounts>(accountInput);
            account.PasswordHash = PasswordHasherWrapper.Hash(accountInput.Password);

            AccountsOutput accountOutput = Mapper.Map<AccountsOutput>(account);

            await AccountsBasePersistence.AddAsync(account);
            return accountOutput;
        }

        public async Task<TokenOutput> SignInAsync(AccountsSignInInput accountInput)
        {
            Accounts? account = await AccountsStaticPersistence.GetByEmailAsync(accountInput.Email) ?? throw new AccountsException("Email not registered.");

            if (!PasswordHasherWrapper.Verify(account.PasswordHash, accountInput.Password))
            {
                throw new AccountsException("Invalid email or password.");
            }

            TokenOutput token = TokenService.GenerateToken(account);
            return token;
        }
    }
}
