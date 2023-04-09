using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.Extensions;
using Trinity.API.ViewModels;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Accounts;
using Trinity.Application.Exceptions.Accounts;

namespace Trinity.API.Controllers.Accounts
{
    [ApiController]
    [Route("v1/[Controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService AccountsService;

        public AccountsController(IAccountsService accountsService)
        {
            AccountsService = accountsService;
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

                AccountsOutput accountCreated = await AccountsService.SignUpAsync(accountInput);
                return StatusCode((int)HttpStatusCode.Created, new ResultViewModel<AccountsOutput>(accountCreated));
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

                TokenOutput token = await AccountsService.SignInAsync(accountInput);
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
