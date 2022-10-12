using Yello.Core.DTOs;
using Yello.Core.DTOs.Team;
using Yello.Core.DTOs.Sprint;
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

    [SwaggerTag("Create a Project Sprint")]
    //[Authorize(Roles = "Team")]
    [Route("api/[controller]")]
    [ApiController]
    public class SprintController : ControllerBase
    {

        private readonly ISprintService _sprintService;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;
        private static readonly HttpClient client = new HttpClient();

        public SprintController(ISprintService sprintService, ITeamService teamService, IUserService userService)
        {
            _teamService = teamService;
            _sprintService = sprintService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var res = await _sprintService.GetByIdAsync(id);
            return Ok(res);
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(SprintCreateDto SprintDto)
        {

            await _sprintService.CreateAsync(SprintDto);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] TeamFilter filter)
        {
            var res = await _teamService.ListAsync(filter);
            return Ok(res);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteAsync(SprintCompleteDto SprintDto)
        {

            await _sprintService.CompleteSprint(SprintDto);
            return Ok();
        }


        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserAsync(int id)
        {
            var res = await _sprintService.GetByUserIdAsync(id);
            return Ok(res);
        }

        //[HttpPost("add-card")]
        //public async Task<IActionResult> AddCardAsync(int sprintId, int cardId)
        //{
        //    await _sprintService.AddCardAsync(sprintId, cardId);
        //    return Ok();
        //}
    }
}
