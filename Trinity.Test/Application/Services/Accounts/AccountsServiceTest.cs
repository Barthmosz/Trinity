using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Accounts;
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
        private AccountsSignInInput accountSignInInput;
        private Accounts? account;
        private AccountsOutput accountOutput;
        private TokenOutput tokenOutput;

        [SetUp]
        public void SetUp()
        {
            this.accountSignUpInput = new()
            {
                Name = "any_name",
                Email = "any_email@mail.com",
                Password = "any_password"
            };
            this.accountSignInInput = new()
            {
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
            this.tokenOutput = new()
            {
                Token = "any_token"
            };

            this.accountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(this.account);
            this.mapper.Setup(m => m.Map<Accounts>(this.accountSignUpInput)).Returns(this.account);
            this.mapper.Setup(m => m.Map<AccountsOutput>(this.account)).Returns(this.accountOutput);
            this.tokenService.Setup(t => t.GenerateToken(this.account)).Returns(this.tokenOutput);
            this.passwordHasher.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>())).Returns(true);

            this.accountsService = new AccountsService(this.accountsStaticPersistence.Object, this.accountsBasePersistence.Object, this.passwordHasher.Object, this.tokenService.Object, this.mapper.Object);
        }

        #region SignUp
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
        #endregion

        #region SignIn
        [Test]
        public void SignInAsync_Should_Throw_If_Email_Does_Not_Exists()
        {
            this.account = null;
            this.accountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(this.account);

            Assert.ThrowsAsync<AccountsException>(async () => await this.accountsService.SignInAsync(this.accountSignInInput));
        }

        [Test]
        public void SignInAsync_Should_Throw_If_Password_Is_Wrong()
        {
            this.passwordHasher.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>())).Returns(false);

            Assert.ThrowsAsync<AccountsException>(async () => await this.accountsService.SignInAsync(this.accountSignInInput));
        }

        [Test]
        public async Task SignInAsync_Should_Return_Token_If_Email_And_Password_Are_Correct()
        {
            TokenOutput result = await this.accountsService.SignInAsync(this.accountSignInInput);
            Assert.That(result, Is.EqualTo(this.tokenOutput));
        }
        #endregion
    }
}
