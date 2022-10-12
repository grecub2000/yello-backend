using Yello.Core.DTOs;
using Yello.Core.DTOs.Team;
using Yello.Core.DTOs.Card;
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

    [SwaggerTag("Create a Sprint Card")]
    //[Authorize(Roles = "Team")]
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {

        private readonly ICardService _cardService;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;
        private static readonly HttpClient client = new HttpClient();

        public CardController(ICardService cardService, ITeamService teamService, IUserService userService)
        {
            _teamService = teamService;
            _cardService = cardService;
            _userService = userService;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAllAsync(int id)
        {
            var res = await _cardService.ListAsync(id);
            return Ok(res);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var res = await _cardService.GetByIdAsync(id);
            return Ok(res);
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CardCreateDto cardDto)
        {

            await _cardService.CreateAsync(cardDto);
            return Ok();
        }

        [HttpPost("update-progress")]
        public async Task<IActionResult> UpdateProgressAsync(int cardId, int progress)
        {

           var res = await _cardService.UpdateCardProgressAsync(cardId, progress);
            return Ok(res);
        }
    }
}
