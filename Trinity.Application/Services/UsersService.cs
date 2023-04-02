using AutoMapper;
using SecureIdentity.Password;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Users;
using Trinity.Domain;
using Trinity.Persistence.Contracts;

namespace Trinity.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IStaticPersistence<Users> usersStaticPersistence;
        private readonly IBasePersistence<Users> usersBasePersistence;
        private readonly IMapper mapper;

        public UsersService(
            IStaticPersistence<Users> usersStaticPersistence,
            IBasePersistence<Users> usersBasePersistence,
            IMapper mapper)
        {
            this.usersStaticPersistence = usersStaticPersistence;
            this.usersBasePersistence = usersBasePersistence;
            this.mapper = mapper;
        }

        public async Task<UsersOutput> AddUserAsync(UsersInput user)
        {
            Users userToBeAdded = this.mapper.Map<Users>(user);
            userToBeAdded.PasswordHash = PasswordHasher.Hash(user.Password);

            UsersOutput userOutput = this.mapper.Map<UsersOutput>(userToBeAdded);

            await this.usersBasePersistence.Add(userToBeAdded);
            return userOutput;
        }
    }
}
