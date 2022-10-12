using Yello.Core.DTOs;
using Yello.Core.DTOs.Team;
using Yello.Core.DTOs.Comment;
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

    [SwaggerTag("Create a Card Comment")]
    //[Authorize(Roles = "Team")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        private readonly ICommentService _commentService;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;
        private static readonly HttpClient client = new HttpClient();

        public CommentController(ICommentService commentService, ITeamService teamService, IUserService userService)
        {
            _teamService = teamService;
            _commentService = commentService;
            _userService = userService;
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var res = await _commentService.GetByIdAsync(id);
            return Ok(res);
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CommentCreateDto commentDto)
        {

            var res = await _commentService.CreateAsync(commentDto);
            return Ok(res);
        }

    }
}
