using Yello.Core.DTOs;
using Yello.Core.DTOs.Team;
using Yello.Core.DTOs.Project;
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
using Yello.Core.Filters;

namespace Yello.Api.Controllers
{

    [SwaggerTag("Create a Team Project, Add Members to it or see all the members from a certain project")]
    //[Authorize(Roles = "Team")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private readonly IProjectService _projectService;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;
        private static readonly HttpClient client = new HttpClient();

        public ProjectController(IProjectService projectService, ITeamService teamService, IUserService userService)
        {
            _teamService = teamService;
            _projectService = projectService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var res = await _projectService.GetByIdAsync(id);
            return Ok(res);
        }

        [HttpPost("add-member")]
        public async Task<IActionResult> AddMemberAsync(int projectId, int userId)
        {
            //var res = await _teamService.GetByIdAsync(id);
            await _projectService.AddMemberAsync(projectId, userId);
            return Ok();
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(ProjectCreateDto projectDto)
        {

            await _projectService.CreateAsync(projectDto);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] ProjectFilter filter)
        {
            var res = await _projectService.ListAsync(filter);
            return Ok(res);
        }
    }
}
