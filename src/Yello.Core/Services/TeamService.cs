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
using Yello.Core.DTOs.Team;

namespace Yello.Core.Services
{
    public class TeamService : ITeamService
    {

        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Team> _teamRepository;
        private readonly IMapper _mapper;

        public TeamService(IGenericRepository<User> userRepository, IGenericRepository<Team> teamRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _mapper = mapper;
            //_jwtHandler = new JwtSecurityTokenHandler();
        }

        public async Task<TeamDto> GetByIdAsync(int id)
        {
            var team = await _teamRepository
                .AsQueryable()
                .Include(x => x.Users)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<TeamDto>(team);
            return result;
        }
        public async Task CreateAsync(TeamCreateDto teamDto)
        {
            var team = _mapper.Map<Team>(teamDto);
            //user.ProfilePicture ??= UserConstants.DefaultProfilePicture;
            //user.KeycloakId = keycloakId;
            //user.RoleId = 1;
            var user = await _userRepository.GetByIdAsync(team.ManagerId);
            team.Manager = user;
            team.Users = new List<User> { user };
            await _teamRepository.AddAsync(team);
        }


        public async Task AddMemberAsync(int teamId, int userId)
        {

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
                throw new CustomNotFoundException("User not found!!!");
            var team = await _teamRepository
                .AsQueryable()
                .Where(x => x.Id == teamId)
                .Include(x => x.Users)
                .FirstOrDefaultAsync();
            if (team is null)
                throw new CustomNotFoundException("Team not found!!!");
            team.Users.Add(user);
            await _teamRepository.SaveChangesAsync();
        }

        public async Task<List<TeamDto>> ListAsync(TeamFilter filter)
        {

            var teams = _teamRepository
                .AsQueryable()
                .ApplyPagination(filter)
                .Include(x => x.Users)
                .Include(x => x.Manager)
                .Include(x => x.Projects)
                //.Where(x => x.KeycloakId == userFilter.KeycloakId)
                //.Where(x => x.Username == userFilter.Username)
                .AsQueryable();
                ;

            if(filter.UserId != 0)
            {
                var user = await _userRepository.GetByIdAsync(filter.UserId);
                teams = teams.Where(x => x.Users.Contains(user));
            }

            var result = await teams.Select(user => _mapper.Map<TeamDto>(user)).ToListAsync();
            return result;

        }
    }
}