using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Accounts;
using Trinity.Application.Exceptions.Accounts;
using Trinity.Application.Services;
using Trinity.Application.Wrappers;
using Trinity.Domain.Entities.Accounts;
using Trinity.Persistence.Contracts;

namespace Trinity.Test.Application.Services.Account
{
    public class AccountsServiceTest
    {
        private readonly Mock<IStaticPersistence<Accounts>> AccountsStaticPersistence = new();
        private readonly Mock<IDynamicPersistence<Accounts>> AccountsBasePersistence = new();
        private readonly Mock<IPasswordHasherWrapper> PasswordHasherWrapper = new();
        private readonly Mock<ITokenService> TokenService = new();
        private readonly Mock<IMapper> Mapper = new();

        private IAccountsService AccountsService;

        private AccountsSignUpInput AccountSignUpInput;
        private AccountsSignInInput AccountSignInInput;
        private Accounts? Account;
        private AccountsOutput AccountOutput;
        private TokenOutput TokenOutput;

        [SetUp]
        public void SetUp()
        {
            AccountSignUpInput = new()
            {
                Name = "any_name",
                Email = "any_email@mail.com",
                Password = "any_password"
            };
            AccountSignInInput = new()
            {
                Email = "any_email@mail.com",
                Password = "any_password"
            };
            AccountOutput = new()
            {
                Id = "any_id",
                Name = "any_name",
                Email = "any_email@mail.com"
            };
            Account = new()
            {
                Name = "any_name",
                Email = "any_email@mail.com",
                PasswordHash = "any_password_hash"
            };
            TokenOutput = new()
            {
                Token = "any_token"
            };

            AccountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(Account);
            Mapper.Setup(m => m.Map<Accounts>(AccountSignUpInput)).Returns(Account);
            Mapper.Setup(m => m.Map<AccountsOutput>(Account)).Returns(AccountOutput);
            TokenService.Setup(t => t.GenerateToken(Account)).Returns(TokenOutput);
            PasswordHasherWrapper.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>())).Returns(true);

            AccountsService = new AccountsService(AccountsStaticPersistence.Object, AccountsBasePersistence.Object, PasswordHasherWrapper.Object, TokenService.Object, Mapper.Object);
        }

        #region SignUp
        [Test]
        public void SignUpAsync_Should_Throw_If_Email_Already_Exists()
        {
            AccountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(Account);

            Assert.ThrowsAsync<AccountsException>(async () => await AccountsService.SignUpAsync(AccountSignUpInput));
        }

        [Test]
        public async Task SignUpAsync_Should_Return_Created_Account()
        {
            Account = null;
            AccountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(Account);

            AccountsOutput result = await AccountsService.SignUpAsync(AccountSignUpInput);
            Assert.That(result, Is.EqualTo(AccountOutput));
        }
        #endregion

        #region SignIn
        [Test]
        public void SignInAsync_Should_Throw_If_Email_Does_Not_Exists()
        {
            Account = null;
            AccountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(Account);

            Assert.ThrowsAsync<AccountsException>(async () => await AccountsService.SignInAsync(AccountSignInInput));
        }

        [Test]
        public void SignInAsync_Should_Throw_If_Password_Is_Wrong()
        {
            PasswordHasherWrapper.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>())).Returns(false);

            Assert.ThrowsAsync<AccountsException>(async () => await AccountsService.SignInAsync(AccountSignInInput));
        }

        [Test]
        public async Task SignInAsync_Should_Return_Token_If_Email_And_Password_Are_Correct()
        {
            TokenOutput result = await AccountsService.SignInAsync(AccountSignInInput);
            Assert.That(result, Is.EqualTo(TokenOutput));
        }
        #endregion
    }
}
