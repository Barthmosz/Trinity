using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Users;
using Trinity.Application.Exceptions.Accounts;
using Trinity.Application.Services;
using Trinity.Application.Wrappers;
using Trinity.Domain.Entities.Accounts;
using Trinity.Persistence.Contracts;

namespace Trinity.Test.Application.Services.Account
{
    public class AccountsServiceTest
    {
        private readonly Mock<IStaticPersistence<Accounts>> accountsStaticPersistence = new();
        private readonly Mock<IBasePersistence<Accounts>> accountsBasePersistence = new();
        private readonly Mock<IPasswordHasherWrapper> passwordHasher = new();
        private readonly Mock<ITokenService> tokenService = new();
        private readonly Mock<IMapper> mapper = new();

        private IAccountsService accountsService;

        private AccountsSignUpInput accountSignUpInput;
        private Accounts? account;
        private AccountsOutput accountOutput;

        [SetUp]
        public void SetUp()
        {
            this.accountSignUpInput = new()
            {
                Name = "any_name",
                Email = "any_email@mail.com",
                Password = "any_password"
            };
            this.accountOutput = new()
            {
                Id = "any_id",
                Name = "any_name",
                Email = "any_email@mail.com"
            };
            this.account = new()
            {
                Name = "any_name",
                Email = "any_email@mail.com",
                PasswordHash = "any_password_hash"
            };

            this.mapper.Setup(m => m.Map<Accounts>(this.accountSignUpInput)).Returns(this.account);
            this.mapper.Setup(m => m.Map<AccountsOutput>(this.account)).Returns(this.accountOutput);

            this.accountsService = new AccountsService(this.accountsStaticPersistence.Object, this.accountsBasePersistence.Object, this.passwordHasher.Object, this.tokenService.Object, this.mapper.Object);
        }

        [Test]
        public void SignUpAsync_Should_Throw_If_Email_Already_Exists()
        {
            this.accountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(this.account);

            Assert.ThrowsAsync<AccountsException>(async () => await this.accountsService.SignUpAsync(this.accountSignUpInput));
        }

        [Test]
        public async Task SignUpAsync_Should_Return_Created_Account()
        {
            this.account = null;
            this.accountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(this.account);

            AccountsOutput result = await this.accountsService.SignUpAsync(this.accountSignUpInput);
            Assert.That(result, Is.EqualTo(this.accountOutput));
        }
    }
}
