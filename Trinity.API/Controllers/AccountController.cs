﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("v1/accounts/signin")]
        public async Task<IActionResult> SignIn([FromBody] AccountsInput account, [FromServices] IAccountsService accountsService)
        {
            try
            {
                string token = await accountsService.SignInAsync(account);
                return StatusCode((int)HttpStatusCode.OK, new ResultViewModel<string>(token, null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultViewModel<IEnumerable<ProductsOutput>>(ex.Message));
            }
        }
    }
}
