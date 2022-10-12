using Yello.Core.Models;
using Yello.Core.DTOs;
using Yello.Core.Interfaces.Services;
using Yello.Core.Interfaces.Repositories;
//using Yello.Core.Services;
using Yello.Keycloak.UserService;
using AutoMapper;
using Yello.Core.Constants;
using Yello.Core.DTOs.Auth;
using Yello.Core.DTOs.User;
using Yello.Keycloak.Models;
using Microsoft.EntityFrameworkCore;
using Yello.Core.Filters;
using Yello.Core.Middlewares.ExceptionMiddleware.CustomExceptions;
using Yello.Pagination;
using Newtonsoft.Json.Linq;
using Yello.Core.DTOs.Card;
using Yello.Core.Enums;

namespace Yello.Core.Services
{
    public class CardService : ICardService
    {

        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Card> _cardRepository;
        private readonly IGenericRepository<Team> _teamRepository;
        private readonly IGenericRepository<Sprint> _sprintRepository;
        private readonly ISprintService _sprintService;
        private readonly IMapper _mapper;

        public CardService(IGenericRepository<User> userRepository, IGenericRepository<Card> cardRepository, IGenericRepository<Team> teamRepository, IGenericRepository<Sprint> sprintRepository, ISprintService sprintService, IMapper mapper)
        {
            _userRepository = userRepository;
            _cardRepository = cardRepository;
            _teamRepository = teamRepository;
            _sprintRepository = sprintRepository;
            _sprintService = sprintService;

            _mapper = mapper;
            //_jwtHandler = new JwtSecurityTokenHandler();
        }

        public async Task<List<CardDto>> ListAsync(int userId)
        {

            var teams = _cardRepository
                .AsQueryable()
                .Include(x => x.Assignee)
                .Include(x => x.Reporter)
                .Include(x => x.Comments)
                .Include(x => x.Sprint)
                .Where(x => x.AssigneeId == userId || x.ReporterId == userId)
                .Where(x => x.Sprint.IsActive == true)
                .AsQueryable();
            ;

            var result = await teams.Select(user => _mapper.Map<CardDto>(user)).ToListAsync();
            return result;

        }


        public async Task<CardDto> GetByIdAsync(int id)
        {
            var card = await _cardRepository
                .AsQueryable()
                .Include(x => x.Assignee)
                .Include(x => x.Reporter)
                //.Include(x => x.Sprints)
                .Include(x => x.Comments)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<CardDto>(card);
            return result;
        }


        public async Task CreateAsync(CardCreateDto cardDto)
        {
            var card = _mapper.Map<Card>(cardDto);
            card.Progress = Enums.CardProgressEnum.ToDo;
            if (card.Created == DateTime.MinValue)
            {
                card.Created = DateTime.Now;
            }
            
            await _cardRepository.AddAsync(card);
            //if (cardDto.SprintId != 0)
            //{
            //    await _sprintService.AddCardAsync(cardDto.SprintId, card.Id);
            //}
        }

        public async Task<CardDto> UpdateCardProgressAsync(int cardId, int progress)
        {
            var card = await _cardRepository.GetByIdAsync(cardId);
            card.Progress = (CardProgressEnum)progress;
            await _cardRepository.SaveChangesAsync();
            return _mapper.Map<CardDto>(card);
        }


    }
}
