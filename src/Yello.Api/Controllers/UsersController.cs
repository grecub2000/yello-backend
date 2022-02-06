using Yello.Core.DTOs;
using Yello.Core.DTOs.User;
using Yello.Core.Filters;
using Yello.Core.Interfaces;
using Yello.Keycloak.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Yello.Api.Controllers
{

    [SwaggerTag("Create a profile, Update it or see other people profiles.")]
    //[Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var res = await _userService.GetByIdAsync(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(UserFilter userFilter)
        {
            var res = await _userService.ListAsync(userFilter);
            return Ok(res);
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(UserRegisterDto registerDto)
        {
            await _userService.RegisterAsync(registerDto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            var res = await _userService.LoginAsync(loginDto);
            return Ok(res);
        }
        //[NonAction]
        //private string CurrentUserId()
        //{
        //    return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //}
    }
}
