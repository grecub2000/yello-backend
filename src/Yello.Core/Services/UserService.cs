using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Yello.Core.Models;
using Yello.Core.DTOs;
using Yello.Core.Interfaces;
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
using Yello.Core.DTOs.Role;

namespace Yello.Core.Services
{
    

    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IKeycloakUserService _keycloakUserService;
        private readonly IMapper _mapper;
        private readonly JwtSecurityTokenHandler _jwtHandler;

        public UserService(IGenericRepository<User> userRepository, IKeycloakUserService keycloakUserService, IMapper mapper, IGenericRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _keycloakUserService = keycloakUserService;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _jwtHandler = new JwtSecurityTokenHandler();
        }

        public async Task<List<UserProfileDto>> ListAsync(UserFilter userFilter)
        {
            var query = _userRepository.AsQueryable();
            if (!String.IsNullOrEmpty(userFilter.KeycloakId))
            {
                //query = query.Where
            }
            //var users = _userRepository
            //    .AsQueryable()
            //    //.ApplyPagination(userFilter)
            //    .Where(x => x.KeycloakId == userFilter.KeycloakId)
            //    .Where(x => x.Username == userFilter.Username)
            //    .AsQueryable();
            //;

            var result = await query.Select(user => _mapper.Map<UserProfileDto>(user)).ToListAsync();
            return result;
        }

        public async Task<List<UserProfileDto>> ListByTeamAsync(int id)
        {

            var users = _userRepository
                .AsQueryable()
                .Include(x => x.Teams)
                //.ApplyPagination
                .Where(x => x.Teams.Any(y => x.Id == id));
            var result = await users.Select(user => _mapper.Map<UserProfileDto>(user)).ToListAsync();
            return result;
        }


        public async Task<UserProfileDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.AsQueryable()
                .Where(x => x.Id == id)
                //.Include(x => x.Comments)
                //.Include(x => x.AssigneeCards)
                //.Include(x => x.ReporterCards)
                //.Include(x => x.Teams).ThenInclude(x => x.Users)
                ////.Include(x => x.Teams).ThenInclude(x => x.Projects)
                //.Include(x => x.TeamsManager)
                //.Include(x => x.Projects)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<UserProfileDto>(user);
            return result;
        }
        public async Task<UserProfileDto> GetInfoByKeycloakId(string keycloakId)
        {
            var user = await _userRepository
                .AsQueryable()
                .Include(x => x.Comments)
                .Include(x => x.AssigneeCards)
                .Include(x => x.ReporterCards)
                .Include(x => x.Teams).ThenInclude(x => x.Users)
                //.Include(x => x.Teams).ThenInclude(x => x.Projects)
                .Include(x => x.TeamsManager)
                .Include(x => x.Projects)
                .FirstOrDefaultAsync(x => x.KeycloakId == keycloakId);
            var result = _mapper.Map<UserProfileDto>(user);
            return result;
        }

        public async Task RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var keycloakUser = _mapper.Map<RegisterDto>(userRegisterDto);
            var res = await _keycloakUserService.RegisterAsync(keycloakUser);
            if (res.HttpStatusCode != HttpStatusCode.Created)
            {
                throw new CustomBadRequestException(res.Content);
            }
            var user = _mapper.Map<User>(userRegisterDto);
            user.ProfilePicture ??= null;
            user.KeycloakId = res.Content;
            user.RoleId = 1;
            await _userRepository.AddAsync(user);
        }
        
        public async Task<LoginResponseDto> GoogleAuthAsync(string token)
        {

            var jsonToken = _jwtHandler.ReadJwtToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var googleId = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            var user = await _userRepository
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.KeycloakId == googleId);
            if (user != null)
            {
                var usr = _mapper.Map<LoginResponseDto>(user);
                usr.AccessToken = token;
                return usr;
            }
            var newUser = new User();
            newUser.KeycloakId = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            newUser.Email = tokenS.Claims.First(claim => claim.Type == "email").Value;
            newUser.Username = tokenS.Claims.First(claim => claim.Type == "email").Value;
            newUser.FirstName = tokenS.Claims.First(claim => claim.Type == "given_name").Value;
            newUser.LastName = tokenS.Claims.First(claim => claim.Type == "family_name").Value;
            newUser.ProfilePicture = tokenS.Claims.First(claim => claim.Type == "picture").Value;
            newUser.RoleId = 1;
            await _userRepository.AddAsync(newUser);

            var ret = _mapper.Map<LoginResponseDto>(newUser);
            ret.AccessToken = token;
            return ret;

        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var res = await _keycloakUserService.LoginAsync(loginDto);
            switch (res.HttpStatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new CustomUnauthorizedException(res.Content);
                default:
                    throw new CustomBadRequestException(res.Content);
            }

            dynamic data = JObject.Parse(res.Content);
            string accessToken = data.access_token;
            var jsonToken = _jwtHandler.ReadJwtToken(accessToken);
            var username = jsonToken.Claims.First(claim => claim.Type == "preferred_username").Value;
            var user = await _userRepository.AsQueryable().Where(x => x.Username == username).FirstOrDefaultAsync();
            var ret = _mapper.Map<LoginResponseDto>(user);
            ret.AccessToken = accessToken;
            return ret;
        }

        public async Task ChangeRoleAsync(RoleChangeDto roleChangeDto)
        {
            var user = await _userRepository.GetByIdAsync(roleChangeDto.UserId);
            var newRole = await _roleRepository.GetByIdAsync(roleChangeDto.RoleId);
            if (user is null || newRole is null)
                throw new CustomNotFoundException("User or Role not found");
            var oldRole = await _roleRepository.GetByIdAsync(user.RoleId);
            await _keycloakUserService.RemoveRoleFromUserAsync(user.KeycloakId, new RoleDto
            {
                Id = oldRole.KeycloakId,
                Name = oldRole.Name
            });
            await _keycloakUserService.AddRoleToUserAsync(user.KeycloakId, new RoleDto
            {
                Id = newRole.KeycloakId,
                Name = newRole.Name
            });
            user.RoleId = newRole.Id;
            await _userRepository.UpdateAsync(user);
        }

        public async Task EditAsync(UserDto user)
        {

            var newUser = await _userRepository.AsQueryable()
                .Where(x => x.Id == user.Id).FirstOrDefaultAsync();

            //var usered = _mapper.Map<User>(user);
            newUser.Email = user.Email;
            newUser.FirstName = user.FirstName;
            newUser.LastName = user.LastName;
            newUser.ProfilePicture = user.ProfilePicture;
            await _userRepository.UpdateAsync(newUser);

        }

    }
}