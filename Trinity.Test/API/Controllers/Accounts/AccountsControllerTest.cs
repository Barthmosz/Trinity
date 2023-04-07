using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.Controllers.Accounts;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Users;
using Trinity.Application.Services;
using Trinity.Domain.Entities.Accounts;
using Trinity.Persistence.Contracts;

namespace Trinity.Test.API.Controllers.Account
{
    public class AccountsControllerTest
    {
        private readonly Mock<IStaticPersistence<Accounts>> accountsStaticPersistence = new();
        private readonly Mock<IBasePersistence<Accounts>> accountsBasePersistence = new();
        private readonly Mock<ITokenService> tokenService = new();
        private readonly Mock<IMapper> mapper = new();

        private IAccountsService accountsService;
        private AccountsController accountsController;

        private AccountsSignUpInput accountsSignUpInput;
        private AccountsOutput accountsOutput;

        private Accounts? account;
        private Accounts accountExists;

        [SetUp]
        public void SetUp()
        {
            this.accountsSignUpInput = new()
            {
                Name = "any_name",
                Email = "any_email@mail.com",
                Password = "any_password"
            };
            this.accountsOutput = new()
            {
                Id = "any_id",
                Name = "any_name",
                Email = "any_email@mail.com"
            };
            this.accountExists = new()
            {
                Name = "any_name",
                Email = "any_email@mail.com"
            };
            this.account = null;

            this.mapper.Setup(m => m.Map<Accounts>(this.accountsSignUpInput)).Returns(this.accountExists);
            this.mapper.Setup(m => m.Map<AccountsOutput>(this.account)).Returns(this.accountsOutput);
            this.accountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(this.account));
            this.accountsBasePersistence.Setup(p => p.AddAsync(It.IsAny<Accounts>())).Returns(Task.FromResult(true));

            this.accountsService = new AccountsService(this.accountsStaticPersistence.Object, this.accountsBasePersistence.Object, this.tokenService.Object, this.mapper.Object);
            this.accountsController = new(this.accountsService);
        }

        #region SignUp
        [Test]
        public async Task SignUp_Should_Return_BadRequest_If_Input_Is_Invalid()
        {
            this.accountsController.ModelState.AddModelError("name", "Name is required.");
            ObjectResult? result = await this.accountsController.SignUp(this.accountsSignUpInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignUp_Should_Return_Created_If_Persistence_Returns_True()
        {
            ObjectResult? result = await this.accountsController.SignUp(this.accountsSignUpInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
        }

        [Test]
        public async Task SignUp_Should_Return_BadRequest_If_Account_Exists()
        {
            this.account = new()
            {
                Name = "any_name",
                Email = "any_email@mail.com"
            };
            this.accountsStaticPersistence.Setup(p => p.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(this.account)!);
            ObjectResult? result = await this.accountsController.SignUp(this.accountsSignUpInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task SignUp_Should_Return_InternalServerError_If_Persistence_Throws()
        {
            this.accountsBasePersistence.Setup(p => p.AddAsync(It.IsAny<Accounts>())).Throws(new Exception());

            ObjectResult? result = await this.accountsController.SignUp(this.accountsSignUpInput) as ObjectResult;
            Assert.That(result!.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
        #endregion
    }
}
