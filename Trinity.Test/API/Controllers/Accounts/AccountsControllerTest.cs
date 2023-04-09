using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.Controllers.Accounts;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Accounts;
using Trinity.Application.Services;
using Trinity.Application.Wrappers;
using Trinity.Domain.Entities.Accounts;
using Trinity.Persistence.Contracts;

namespace Trinity.Test.API.Controllers.Account
{
    public class AccountsControllerTest
    {
        private readonly Mock<IStaticPersistence<Accounts>> AccountsStaticPersistence = new();
        private readonly Mock<IDynamicPersistence<Accounts>> AccountsBasePersistence = new();
        private readonly Mock<IPasswordHasherWrapper> PasswordHasherWrapper = new();
        private readonly Mock<ITokenService> TokenService = new();
        private readonly Mock<IMapper> Mapper = new();

        private IAccountsService AccountsService;
        private AccountsController AccountsController;

        private AccountsSignUpInput AccountsSignUpInput;
        private AccountsSignInInput AccountsSignInInput;
        private AccountsOutput AccountsOutput;

        private Accounts? Account;
        private Accounts AccountExists;

        [SetUp]
        public void SetUp()
        {
            AccountsSignUpInput = new()
            {
                Name = "any_name",
                Email = "any_email@mail.com",
                Password = "any_password"
            };
            AccountsSignInInput = new()
            {
                Email = "any_email@mail.com",
                Password = "any_password"
            };
            AccountsOutput = new()
            {
                Id = "any_id",
                Name = "any_name",
                Email = "any_email@mail.com"
            };
            AccountExists = new()
            {
                Name = "any_name",
                Email = "any_email@mail.com"
            };
            Account = null;

            Mapper.Setup(m => m.Map<Accounts>(AccountsSignUpInput)).Returns(AccountExists);
            Mapper.Setup(m => m.Map<AccountsOutput>(Account)).Returns(AccountsOutput);
            AccountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(Account));
            AccountsBasePersistence.Setup(p => p.AddAsync(It.IsAny<Accounts>())).Returns(Task.FromResult(true));
            PasswordHasherWrapper.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>())).Returns(true);

            AccountsService = new AccountsService(AccountsStaticPersistence.Object, AccountsBasePersistence.Object, PasswordHasherWrapper.Object, TokenService.Object, Mapper.Object);
            AccountsController = new(AccountsService);
        }

        #region SignUp
        [Test]
        public async Task SignUp_Should_Return_BadRequest_If_Input_Is_Invalid()
        {
            AccountsController.ModelState.AddModelError("name", "Name is required.");
            ObjectResult? result = await AccountsController.SignUp(AccountsSignUpInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignUp_Should_Return_Created_If_Persistence_Returns_True()
        {
            ObjectResult? result = await AccountsController.SignUp(AccountsSignUpInput) as ObjectResult;
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
            AccountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(Account)!);
            ObjectResult? result = await AccountsController.SignUp(AccountsSignUpInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignUp_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            AccountsBasePersistence.Setup(p => p.AddAsync(It.IsAny<Accounts>())).Throws(new Exception());

            ObjectResult? result = await AccountsController.SignUp(AccountsSignUpInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion

        #region SignIn
        [Test]
        public async Task SignIn_Should_Return_BadRequest_If_Input_Is_Invalid()
        {
            AccountsController.ModelState.AddModelError("name", "Name is required.");
            ObjectResult? result = await AccountsController.SignIn(AccountsSignInInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignIn_Should_Return_Ok_If_Email_And_Password_Are_Valid()
        {
            AccountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(AccountExists)!);
            ObjectResult? result = await AccountsController.SignIn(AccountsSignInInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task SignIn_Should_Return_BadRequest_If_Email_Is_Not_Registered()
        {
            AccountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(Account));
            ObjectResult? result = await AccountsController.SignIn(AccountsSignInInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignIn_Should_Return_BadRequest_If_Password_Is_Not_Correct()
        {
            AccountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(AccountExists)!);
            PasswordHasherWrapper.Setup(p => p.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<short>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>())).Returns(false);
            ObjectResult? result = await AccountsController.SignIn(AccountsSignInInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignIn_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            AccountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Throws(new Exception());

            ObjectResult? result = await AccountsController.SignIn(AccountsSignInInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion
    }
}
