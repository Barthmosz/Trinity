using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Trinity.API.ViewModels;
using Trinity.Application.Contracts;
using Trinity.Application.DTOs.Products;
using Trinity.Application.DTOs.Users;

namespace Trinity.API.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("v1/accounts/signup")]
        public async Task<IActionResult> SignUp([FromBody] UsersInput user, [FromServices] IUsersService usersService)
        {
            try
            {
                UsersOutput userCreated = await usersService.AddUserAsync(user);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<UsersOutput>(userCreated));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<IEnumerable<ProductsOutput>>(ex.Message));
            }
        }
    }
}
