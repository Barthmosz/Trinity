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
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] AccountsSignUpInput accountInput, [FromServices] IAccountsService accountsService)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<AccountsOutput>(ModelState.GetErrors()));
                }

                AccountsOutput userCreated = await accountsService.SignUpAsync(accountInput);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<AccountsOutput>(userCreated));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<IEnumerable<ProductsOutput>>(ex.Message));
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] AccountsSignInInput accountInput, [FromServices] IAccountsService accountsService)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultViewModel<AccountsOutput>(ModelState.GetErrors()));
                }

                TokenOutput token = await accountsService.SignInAsync(accountInput);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<TokenOutput>(token));
            }
            catch (AccountsException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ResultViewModel<IEnumerable<ProductsOutput>>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<IEnumerable<ProductsOutput>>(ex.Message));
            }
        }
    }
}
