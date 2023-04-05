using AutoMapper;
using SecureIdentity.Password;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Accounts;
using Trinity.Application.DTOs.Users;
using Trinity.Application.Exceptions.Accounts;
using Trinity.Domain.Entities.Accounts;
using Trinity.Persistence.Contracts;

namespace Trinity.Application.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IStaticPersistence<Accounts> usersStaticPersistence;
        private readonly IBasePersistence<Accounts> usersBasePersistence;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountsService(
            IStaticPersistence<Accounts> usersStaticPersistence,
            IBasePersistence<Accounts> usersBasePersistence,
            ITokenService tokenService,
            IMapper mapper)
        {
            this.usersStaticPersistence = usersStaticPersistence;
            this.usersBasePersistence = usersBasePersistence;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<AccountsOutput> SignUpAsync(AccountsSignUpInput accountInput)
        {
            Accounts? accountExists = await this.usersStaticPersistence.GetByEmailAsync(accountInput.Email);

            if (accountExists != null)
            {
                throw new AccountsException("Email already registered.");
            }

            Accounts account = this.mapper.Map<Accounts>(accountInput);
            account.PasswordHash = PasswordHasher.Hash(accountInput.Password);

            AccountsOutput accountOutput = this.mapper.Map<AccountsOutput>(account);

            await this.usersBasePersistence.Add(account);
            return accountOutput;
        }

        public async Task<TokenOutput> SignInAsync(AccountsSignInInput accountInput)
        {
            Accounts? account = await this.usersStaticPersistence.GetByEmailAsync(accountInput.Email) ?? throw new AccountsException("Email not registered.");

            if (!PasswordHasher.Verify(account.PasswordHash, accountInput.Password))
            {
                throw new AccountsException("Invalid email or password.");
            }

            TokenOutput token = this.tokenService.GenerateToken(account);
            return token;
        }
    }
}
