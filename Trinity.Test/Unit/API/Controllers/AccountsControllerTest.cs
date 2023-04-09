using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.Controllers;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Account;
using Trinity.Application.Services;
using Trinity.Application.Wrappers;
using Trinity.Domain.Entities;
using Trinity.Persistence.Contracts;
using Trinity.Test.Factories;

namespace Trinity.Test.Unit.API.Controllers
{
    [TestFixture]
    public class AccountsControllerTest
    {
        private readonly Mock<IStaticPersistence<Account>> AccountStaticPersistence = new();
        private readonly Mock<IDynamicPersistence<Account>> AccountBasePersistence = new();
        private readonly Mock<IPasswordHasherWrapper> PasswordHasherWrapper = new();
        private readonly Mock<ITokenService> TokenService = new();
        private readonly Mock<IMapper> Mapper = new();

        private IAccountService AccountService;
        private AccountController AccountController;

        private AccountSignUpInput AccountSignUpInput;
        private AccountSignInInput AccountSignInInput;
        private AccountOutput AccountOutput;

        private Account? Account;
        private Account AccountExists;

        [SetUp]
        public void SetUp()
        {
            AccountSignUpInput = AccountFactory.MakeAccountSignUpInput();
            AccountSignInInput = AccountFactory.MakeAccountSignInInput();
            AccountOutput = AccountFactory.MakeAccountOutput();
            AccountExists = AccountFactory.MakeAccount();
            Account = null;

            Mapper.Setup(m => m.Map<Account>(AccountSignUpInput)).Returns(AccountExists);
            Mapper.Setup(m => m.Map<AccountOutput>(Account)).Returns(AccountOutput);
            AccountStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(Account));
            AccountBasePersistence.Setup(p => p.AddAsync(It.IsAny<Account>())).Returns(Task.FromResult(true));
            PasswordHasherWrapper.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>())).Returns(true);

            AccountService = new AccountService(AccountStaticPersistence.Object, AccountBasePersistence.Object, PasswordHasherWrapper.Object, TokenService.Object, Mapper.Object);
            AccountController = new(AccountService);
        }

        #region SignUp
        [Test]
        public async Task SignUp_Should_Return_BadRequest_If_Input_Is_Invalid()
        {
            AccountController.ModelState.AddModelError("name", "Name is required.");
            ObjectResult? result = await AccountController.SignUp(AccountSignUpInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignUp_Should_Return_Created_If_Persistence_Returns_True()
        {
            ObjectResult? result = await AccountController.SignUp(AccountSignUpInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
        }

        [Test]
        public async Task SignUp_Should_Return_BadRequest_If_Account_Exists()
        {
            Account = new()
            {
                Name = "any_name",
                Email = "any_email@mail.com"
            };
            AccountStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(Account)!);
            ObjectResult? result = await AccountController.SignUp(AccountSignUpInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignUp_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            AccountBasePersistence.Setup(p => p.AddAsync(It.IsAny<Account>())).Throws(new Exception());

            ObjectResult? result = await AccountController.SignUp(AccountSignUpInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion

        #region SignIn
        [Test]
        public async Task SignIn_Should_Return_BadRequest_If_Input_Is_Invalid()
        {
            AccountController.ModelState.AddModelError("name", "Name is required.");
            ObjectResult? result = await AccountController.SignIn(AccountSignInInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignIn_Should_Return_Ok_If_Email_And_Password_Are_Valid()
        {
            AccountStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(AccountExists)!);
            ObjectResult? result = await AccountController.SignIn(AccountSignInInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task SignIn_Should_Return_BadRequest_If_Email_Is_Not_Registered()
        {
            AccountStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(Account));
            ObjectResult? result = await AccountController.SignIn(AccountSignInInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignIn_Should_Return_BadRequest_If_Password_Is_Not_Correct()
        {
            AccountStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(AccountExists)!);
            PasswordHasherWrapper.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>())).Returns(false);
            ObjectResult? result = await AccountController.SignIn(AccountSignInInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignIn_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            AccountStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Throws(new Exception());

            ObjectResult? result = await AccountController.SignIn(AccountSignInInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion
    }
}
