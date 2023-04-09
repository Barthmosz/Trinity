using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.Extensions;
using Trinity.API.ViewModels;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Account;
using Trinity.Application.Exceptions;

namespace Trinity.API.Controllers
{
    [ApiController]
    [Route("v1/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService AccountService;

        public AccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] AccountSignUpInput accountSignUpInput)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<AccountOutput>(ModelState.GetErrors()));
                }

                AccountOutput accountCreated = await AccountService.SignUpAsync(accountSignUpInput);
                return StatusCode((int)HttpStatusCode.Created, new ResultViewModel<AccountOutput>(accountCreated));
            }
            catch (AccountException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ResultViewModel<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<string>(ex.Message));
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] AccountSignInInput accountSignInInput)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<AccountOutput>(ModelState.GetErrors()));
                }

                TokenOutput token = await AccountService.SignInAsync(accountSignInInput);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<TokenOutput>(token));
            }
            catch (AccountException ex)
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
