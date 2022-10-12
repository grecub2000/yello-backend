using Yello.Core.DTOs;
using Yello.Core.DTOs.User;
using Yello.Core.Filters;
using Yello.Core.Interfaces;
using Yello.Keycloak.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Yello.Core.DTOs.Role;
using System.Net.Http;
using System.Security.Claims;

namespace Yello.Api.Controllers
{

    [SwaggerTag("Create a profile, Update it or see other people profiles.")]
    //[Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private static readonly HttpClient client = new HttpClient();

        public UserController(IUserService userService)
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
        public async Task<IActionResult> GetAllAsync([FromQuery] UserFilter userFilter)
        {
            var res = await _userService.ListAsync(userFilter);
            return Ok(res);
        }

        [Authorize]
        [HttpGet("check-user")]
        public async Task<IActionResult> GetOwnInfoAsync()
        {
            var keycloakId = User.FindFirst(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
            var res = await _userService.GetInfoByKeycloakId(keycloakId);
            if(res == null)
            {
                return Ok("User not found");
            }
            return Ok(res);
        }

        //[Authorize]
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

        [Authorize(Roles = "Admin")]
        [HttpPost("role")]
        public async Task<IActionResult> ChangeRoleAsync(RoleChangeDto roleChangeDto)
        {
            await _userService.ChangeRoleAsync(roleChangeDto);
            return Ok();
        }

        [HttpGet("google-auth")]
        public async Task<IActionResult> GoogleAuthAsync([FromHeader]string authorize)
        {
            //var x=  HttpContext.Request.Headers.Authorization;
            var res = await _userService.GoogleAuthAsync(authorize);
            return Ok(res);
        }

        [HttpPost("edit")]

        public async Task<IActionResult> EditAsync(UserDto user)
        {
            await _userService.EditAsync(user);
            return Ok();
        }

    }
}
