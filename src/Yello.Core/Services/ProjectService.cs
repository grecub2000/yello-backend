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
using Yello.Core.DTOs.Project;

namespace Yello.Core.Services
{
    public class ProjectService : IProjectService
    {

        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Project> _projectRepository;
        private readonly IGenericRepository<Team> _teamRepository;
        private readonly IGenericRepository<Sprint> _sprintRepository;
        private readonly IMapper _mapper;

        public ProjectService(IGenericRepository<User> userRepository, IGenericRepository<Project> projectRepository, IGenericRepository<Team> teamRepository, IGenericRepository<Sprint> sprintRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
            _sprintRepository = sprintRepository;
            _mapper = mapper;
            //_jwtHandler = new JwtSecurityTokenHandler();
        }

        public async Task<ProjectDto> GetByIdAsync(int id)
        {
            var project = await _projectRepository
                .AsQueryable()
                //.Include(x => x.Users)
                //.Include(x => x.Team)
                //.Include(x => x.Sprints)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<ProjectDto>(project);
            return result;
        }


        public async Task CreateAsync(ProjectCreateDto projectDto)
        {

            var user = await _userRepository
                .AsQueryable()
                .Where(x => x.Id == 3)
                .FirstOrDefaultAsync();
            var project = _mapper.Map<Project>(projectDto);
            project.Users = new List<User> { user };
            await _projectRepository.AddAsync(project);
            var sprint = new Sprint();
            sprint.Descrption = "First Sprint";
            sprint.ProjectId = project.Id;
            sprint.IsActive = true;
            await _sprintRepository.AddAsync(sprint);
        }

        public async Task AddMemberAsync(int projectId, int userId)
        {

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
                throw new CustomNotFoundException("User not found!!!");
            var project = await _projectRepository
                .AsQueryable()
                .Where(x => x.Id == projectId)
                .Include(x => x.Users)
                .FirstOrDefaultAsync();
            if (project is null)
                throw new CustomNotFoundException("Project not found!!!");
            var team = await _teamRepository
                .AsQueryable()
                .Where(x => x.Id == project.TeamId)
                .Include(x => x.Users)
                .FirstOrDefaultAsync();
            if (!team.Users.Contains(user))
                throw new CustomNotFoundException("User is not in the project team!!!");

            project.Users.Add(user);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task<List<ProjectDto>> ListAsync(ProjectFilter filter)
        {

            var projects = _projectRepository
                .AsQueryable()
                .ApplyPagination(filter)
                .Include(x => x.Sprints).ThenInclude(x => x.Cards)
                .Include(x => x.Users)
                //.Where(x => x.KeycloakId == userFilter.KeycloakId)
                //.Where(x => x.Username == userFilter.Username)
                .AsQueryable();
            ;

            if (filter.UserId != 0)
            {
                var user = await _userRepository.GetByIdAsync(filter.UserId);
                projects = projects.Where(x => x.Users.Contains(user));
            }
            var result = await projects.Select(project=> _mapper.Map<ProjectDto>(project)).ToListAsync();
            return result;
        }
    }
}
