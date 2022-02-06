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

namespace Yello.Core.Services
{
    

    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IKeycloakUserService _keycloakUserService;
        private readonly IMapper _mapper;
        private readonly JwtSecurityTokenHandler _jwtHandler;

        public UserService(IGenericRepository<User> userRepository, IKeycloakUserService keycloakUserService, IMapper mapper)
        {
            _userRepository = userRepository;
            _keycloakUserService = keycloakUserService;
            _mapper = mapper;
            _jwtHandler = new JwtSecurityTokenHandler();
        }
        
        public async Task<List<UserDto>> ListAsync(UserFilter userFilter)
        {
            
            var users = _userRepository
                .ListAllAsQueryable()
                .ApplyPagination(userFilter)
                ;

            var result = await users.Select(user => _mapper.Map<UserDto>(user)).ToListAsync();
            return result;
        }
        public async Task<UserProfileDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.ListAllAsQueryable()
                .Where(x => x.Id == id)
                //.Include(x => x.FeaturedCourse)
                //.Include(x => x.CreatedCourses)
                //.Include(x => x.Courses)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<UserProfileDto>(user);
            return result;
        }

        public async Task RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var keycloakUser = _mapper.Map<RegisterDto>(userRegisterDto);
            var res = await _keycloakUserService.Register(keycloakUser);
            var user = _mapper.Map<User>(userRegisterDto);
            user.ProfilePicture ??= UserConstants.DefaultProfilePicture;
            await _userRepository.AddAsync(user);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var res = await _keycloakUserService.Login(loginDto);
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
            var user = await _userRepository.ListAllAsQueryable().Where(x => x.Username == username).FirstOrDefaultAsync();
            var ret = _mapper.Map<LoginResponseDto>(user);
            ret.AccessToken = accessToken;
            return ret;
        }
    }
}
