using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isaac_AuthorizationService.Helpers;
using Isaac_AuthorizationService.Interfaces;
using Isaac_AuthorizationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Isaac_AuthorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            try
            {
                _userService.Create(user);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(User user)
        {
            if (user.Email != "" && user.Password != "")
            {
                var jwtUser = _userService.Authenticate(user.Email, user.Password);
                if (jwtUser == null)
                {
                    return BadRequest();
                }
                jwtUser.User = jwtUser.User.WithoutPassword();
                return Ok(jwtUser);
            }
            return BadRequest();
        }
        
        [HttpGet("")]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

        [HttpPost("{id}/delete")]
        //Delete user
        public IActionResult Delete([FromRoute]int id)
        {
            _userService.Delete(id);
            return Ok();
        }
        
        [HttpPost("{id}/update")]
        //Delete user
        public IActionResult Update([FromRoute]int id, [FromBody] User user)
        {
            _userService.Update(user);
            return Ok();
        }
    }
}
