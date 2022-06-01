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

            var users = _userRepository
                .AsQueryable()
                .ApplyPagination(userFilter)
                .Where(x => x.KeycloakId == userFilter.KeycloakId)
                .Where(x => x.Username == userFilter.Username)
                ;

            var result = await users.Select(user => _mapper.Map<UserProfileDto>(user)).ToListAsync();
            return result;
        }
        public async Task<UserProfileDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.AsQueryable()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<UserProfileDto>(user);
            return result;
        }
        public async Task<UserProfileDto> GetInfoByKeycloakId(string keycloakId)
        {
            var user = await _userRepository
                .AsQueryable()
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.KeycloakId == keycloakId);
            var result = _mapper.Map<UserProfileDto>(user);
            return result;
        }

        public async Task RegisterAsync(UserRegisterDto userRegisterDto, string keycloakId)
        {
            //var keycloakUser = _mapper.Map<RegisterDto>(userRegisterDto);
            //var res = await _keycloakUserService.RegisterAsync(keycloakUser);
            //if (res.HttpStatusCode != HttpStatusCode.Created)
            //{
            //throw new CustomBadRequestException(res.Content);
            //}
            var user = _mapper.Map<User>(userRegisterDto);
            //user.ProfilePicture ??= UserConstants.DefaultProfilePicture;
            user.KeycloakId = keycloakId;
            user.RoleId = 1;
            await _userRepository.AddAsync(user);
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
    }
}