using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Account;
using Trinity.Application.Exceptions;
using Trinity.Application.Services;
using Trinity.Application.Wrappers;
using Trinity.Domain.Entities;
using Trinity.Persistence.Contracts;
using Trinity.Test.Factories;

namespace Trinity.Test.Unit.Application.Services
{
    [TestFixture]
    public class AccountServiceTest
    {
        private readonly Mock<IStaticPersistence<Account>> AccountStaticPersistence = new();
        private readonly Mock<IDynamicPersistence<Account>> AccountBasePersistence = new();
        private readonly Mock<IPasswordHasherWrapper> PasswordHasherWrapper = new();
        private readonly Mock<ITokenService> TokenService = new();
        private readonly Mock<IMapper> Mapper = new();

        private IAccountService AccountService;

        private AccountSignUpInput AccountSignUpInput;
        private AccountSignInInput AccountSignInInput;
        private AccountOutput AccountOutput;
        private TokenOutput TokenOutput;

        private Account? Account;

        [SetUp]
        public void SetUp()
        {
            AccountSignUpInput = AccountFactory.MakeAccountSignUpInput();
            AccountSignInInput = AccountFactory.MakeAccountSignInInput();
            AccountOutput = AccountFactory.MakeAccountOutput();
            Account = AccountFactory.MakeAccount();
            TokenOutput = new()
            {
                Token = "any_token"
            };

            AccountStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(Account);
            Mapper.Setup(m => m.Map<Account>(AccountSignUpInput)).Returns(Account);
            Mapper.Setup(m => m.Map<AccountOutput>(Account)).Returns(AccountOutput);
            TokenService.Setup(t => t.GenerateToken(Account)).Returns(TokenOutput);
            PasswordHasherWrapper.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>())).Returns(true);

            AccountService = new AccountService(AccountStaticPersistence.Object, AccountBasePersistence.Object, PasswordHasherWrapper.Object, TokenService.Object, Mapper.Object);
        }

        #region SignUp
        [Test]
        public void SignUpAsync_Should_Throw_If_Email_Already_Exists()
        {
            AccountStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(Account);

            Assert.ThrowsAsync<AccountException>(async () => await AccountService.SignUpAsync(AccountSignUpInput));
        }

        [Test]
        public async Task SignUpAsync_Should_Return_Created_Account()
        {
            Account = null;
            AccountStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(Account);

            AccountOutput result = await AccountService.SignUpAsync(AccountSignUpInput);
            Assert.That(result, Is.EqualTo(AccountOutput));
        }
        #endregion

        #region SignIn
        [Test]
        public void SignInAsync_Should_Throw_If_Email_Does_Not_Exists()
        {
            Account = null;
            AccountStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(Account);

            Assert.ThrowsAsync<AccountException>(async () => await AccountService.SignInAsync(AccountSignInInput));
        }

        [Test]
        public void SignInAsync_Should_Throw_If_Password_Is_Wrong()
        {
            PasswordHasherWrapper.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>())).Returns(false);

            Assert.ThrowsAsync<AccountException>(async () => await AccountService.SignInAsync(AccountSignInInput));
        }

        [Test]
        public async Task SignInAsync_Should_Return_Token_If_Email_And_Password_Are_Correct()
        {
            TokenOutput result = await AccountService.SignInAsync(AccountSignInInput);
            Assert.That(result, Is.EqualTo(TokenOutput));
        }
        #endregion
    }
}
