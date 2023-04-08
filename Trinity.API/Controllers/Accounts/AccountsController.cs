using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.Extensions;
using Trinity.API.ViewModels;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Accounts;
using Trinity.Application.DTOs.Products;
using Trinity.Application.DTOs.Users;
using Trinity.Application.Exceptions.Accounts;

namespace Trinity.API.Controllers.Accounts
{
    [ApiController]
    [Route("v1/[Controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            this.accountsService = accountsService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] AccountsSignUpInput accountInput)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<AccountsOutput>(ModelState.GetErrors()));
                }

                AccountsOutput userCreated = await this.accountsService.SignUpAsync(accountInput);
                return StatusCode((int)HttpStatusCode.Created, new ResultViewModel<AccountsOutput>(userCreated));
            }
            catch (AccountsException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ResultViewModel<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] AccountsSignInInput accountInput)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<AccountsOutput>(ModelState.GetErrors()));
                }

                TokenOutput token = await this.accountsService.SignInAsync(accountInput);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<TokenOutput>(token));
            }
            catch (AccountsException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ResultViewModel<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<string>(ex.Message));
            }
        }
    }
}
