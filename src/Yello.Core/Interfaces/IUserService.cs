using Yello.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.DTOs.Auth;
using Yello.Core.DTOs.User;
using Yello.Core.Filters;
using Yello.Keycloak.Models;

namespace Yello.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetByIdAsync(int id);
        Task RegisterAsync(UserRegisterDto userRegisterDto);
        Task<List<UserDto>> ListAsync(UserFilter userFilter);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    }
}
