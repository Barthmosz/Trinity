using AutoMapper;
using SecureIdentity.Password;
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
        private readonly IStaticPersistence<Accounts> accountsStaticPersistence;
        private readonly IDynamicPersistence<Accounts> accountsBasePersistence;
        private readonly IPasswordHasherWrapper passwordHasher;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountsService(
            IStaticPersistence<Accounts> accountsStaticPersistence,
            IDynamicPersistence<Accounts> accountsBasePersistence,
            IPasswordHasherWrapper passwordHasher,
            ITokenService tokenService,
            IMapper mapper)
        {
            this.accountsStaticPersistence = accountsStaticPersistence;
            this.accountsBasePersistence = accountsBasePersistence;
            this.passwordHasher = passwordHasher;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<AccountsOutput> SignUpAsync(AccountsSignUpInput accountInput)
        {
            Accounts? accountExists = await this.accountsStaticPersistence.GetByEmailAsync(accountInput.Email);

            if (accountExists != null)
            {
                throw new AccountsException("Email already registered.");
            }

            Accounts account = this.mapper.Map<Accounts>(accountInput);
            account.PasswordHash = PasswordHasher.Hash(accountInput.Password);

            AccountsOutput accountOutput = this.mapper.Map<AccountsOutput>(account);

            await this.accountsBasePersistence.AddAsync(account);
            return accountOutput;
        }

        public async Task<TokenOutput> SignInAsync(AccountsSignInInput accountInput)
        {
            Accounts? account = await this.accountsStaticPersistence.GetByEmailAsync(accountInput.Email) ?? throw new AccountsException("Email not registered.");

            if (!this.passwordHasher.Verify(account.PasswordHash, accountInput.Password))
            {
                throw new AccountsException("Invalid email or password.");
            }

            TokenOutput token = this.tokenService.GenerateToken(account);
            return token;
        }
    }
}
