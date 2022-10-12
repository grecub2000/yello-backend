using Yello.Core.DTOs;
using Yello.Core.DTOs.Team;
using Yello.Core.Filters;
using Yello.Core.Interfaces.Services;
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

    [SwaggerTag("Create a Team, Add Members to it or see all the members from a certain team")]
    //[Authorize(Roles = "Team")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;
        private static readonly HttpClient client = new HttpClient();

        public TeamController(ITeamService teamService, IUserService userService)
        {
            _teamService = teamService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] TeamFilter filter)
        {
            var res = await _teamService.ListAsync(filter);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var res = await _teamService.GetByIdAsync(id);
            return Ok(res);
        }

        [HttpPost("add-member")]
        public async Task<IActionResult> AddMemberAsync(int teamId, int userId)
        {
            //var res = await _teamService.GetByIdAsync(id);
            await _teamService.AddMemberAsync(teamId, userId);
            return Ok();
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(TeamCreateDto teamDto)
        {
            if (teamDto.ManagerId == 0)
            {
                var keycloakId = User.FindFirst(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
                var user = await _userService.GetInfoByKeycloakId(keycloakId);
                teamDto.ManagerId = user.Id;
            }
            await _teamService.CreateAsync(teamDto);
            return Ok();
        }
    }
}