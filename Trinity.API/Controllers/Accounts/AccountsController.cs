using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
        public async Task<IActionResult> SignUp([FromBody] AccountsInput account, [FromServices] IAccountsService accountsService)
        {
            try
            {
                AccountsOutput userCreated = await accountsService.SignUpAsync(account);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<AccountsOutput>(userCreated));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<IEnumerable<ProductsOutput>>(ex.Message));
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] AccountsInput account, [FromServices] IAccountsService accountsService)
        {
            try
            {
                TokenOutput token = await accountsService.SignInAsync(account);
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
