using AutoMapper;
using SecureIdentity.Password;
using System;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Users;
using Trinity.Domain.Accounts;
using Trinity.Persistence.Contracts;

namespace Trinity.Application.Services
{
    public class AccountsService : IUsersService
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

        public async Task<UsersOutput> AddUserAsync(UsersInput user)
        {
            Accounts userToBeAdded = this.mapper.Map<Accounts>(user);
            userToBeAdded.PasswordHash = PasswordHasher.Hash(user.Password);

            UsersOutput userOutput = this.mapper.Map<UsersOutput>(userToBeAdded);

            await this.usersBasePersistence.Add(userToBeAdded);
            return userOutput;
        }

        public async Task<string> SignInUserAsync(UsersInput userInput)
        {
            Accounts? user = await this.usersStaticPersistence.GetByEmailAsync(userInput.Email) ?? throw new Exception("Email not registered.");

            if (!PasswordHasher.Verify(user.PasswordHash, userInput.Password))
            {
                throw new Exception("User or password invalid.");
            }

            string token = this.tokenService.GenerateToken(user);
            return token;
        }
    }
}
