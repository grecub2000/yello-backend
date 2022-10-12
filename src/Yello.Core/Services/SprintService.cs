using Yello.Core.Models;
using Yello.Core.DTOs;
using Yello.Core.Interfaces.Services;
using Yello.Core.Interfaces.Repositories;
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
using Yello.Core.DTOs.Sprint;

namespace Yello.Core.Services
{
    public class SprintService : ISprintService
    {

        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Sprint> _sprintRepository;
        private readonly IGenericRepository<Team> _teamRepository;
        private readonly IGenericRepository<Card> _cardRepository;

        private readonly IMapper _mapper;

        public SprintService(IGenericRepository<User> userRepository, IGenericRepository<Sprint> sprintRepository, IGenericRepository<Team> teamRepository, IGenericRepository<Card> cardRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _sprintRepository = sprintRepository;
            _teamRepository = teamRepository;
            _cardRepository = cardRepository;


            _mapper = mapper;
            //_jwtHandler = new JwtSecurityTokenHandler();
        }

        public async Task<SprintDto> GetByIdAsync(int id)
        {
            var sprint = await _sprintRepository
                .AsQueryable()
                //.Include(x => x.Project)
                .Include(x => x.Cards)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<SprintDto>(sprint);
            return result;
        }


        public async Task CreateAsync(SprintCreateDto sprintDto)
        {
            var sprint = _mapper.Map<Sprint>(sprintDto);
            sprint.IsActive = true;
            await _sprintRepository.AddAsync(sprint);
        }

        public async Task<List<SprintInfoDto>> GetByUserIdAsync(int id)
        {

            var user = await _userRepository.GetByIdAsync(id);
            var sprints = _sprintRepository
                .AsQueryable()
                //.Include(x => x.Project)
                .Include(x => x.Project)
                .ThenInclude(x => x.Users)
                //.Include(x => x.Cards)
                .Where(x => x.Project.Users.Contains(user) == true);
            var result = await sprints.Select(sprint => _mapper.Map<SprintInfoDto>(sprint)).ToListAsync();
            return result;

        }

        //public async Task AddCardAsync(int sprintId, int cardId)
        //{

        //    var card = await _cardRepository.GetByIdAsync(cardId);
        //    if (card is null)
        //        throw new CustomNotFoundException("Card not found!!!");
        //    //var team = await _teamRepository
        //    //    .AsQueryable()
        //    //    .Where(x => x.Id == teamId)
        //    //    .Include(x => x.Users)
        //    //    .FirstOrDefaultAsync();
        //    var sprint = await _sprintRepository.GetByIdAsync(sprintId);
        //    if (sprint is null)
        //        throw new CustomNotFoundException("Sprint not found!!!");
        //    if(sprint.Cards is null)
        //        sprint.Cards = new List<Card> { };
        //    sprint.Cards.Add(card);
        //    await _sprintRepository.SaveChangesAsync();
        //}

        public async Task CompleteSprint(SprintCompleteDto sprintDto)
        {

            var sprint = _mapper.Map<Sprint>(sprintDto);
            var prevSprint = await _sprintRepository
                .AsQueryable()
                //.Include(x => x.Project)
                .Include(x => x.Cards)
                .Where(x => x.Id == sprintDto.PreviousSprintId)
                .FirstOrDefaultAsync();
            sprint.ProjectId = prevSprint.ProjectId;
            if (sprintDto.IncludeCards)
            {
                var cards = prevSprint.Cards.
                    AsQueryable()
                    .Where(x => (x.Progress != Enums.CardProgressEnum.Blocked))
                    .Where(x => (x.Progress != Enums.CardProgressEnum.Done))
                    .ToList();

                sprint.Cards = cards;
            }
            prevSprint.IsActive = false;
            sprint.IsActive = true;
            await _sprintRepository.AddAsync(sprint);
            await _sprintRepository.UpdateAsync(prevSprint);
        }

    }
}
