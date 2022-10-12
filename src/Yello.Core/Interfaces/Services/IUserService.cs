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
using Yello.Core.DTOs.Role;

namespace Yello.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetByIdAsync(int id);
        Task<UserProfileDto> GetInfoByKeycloakId(string keycloakId);
        Task RegisterAsync(UserRegisterDto userRegisterDto);
        Task<List<UserProfileDto>> ListAsync(UserFilter userFilter);
        Task<List<UserProfileDto>> ListByTeamAsync(int id);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<LoginResponseDto> GoogleAuthAsync(string token);
        Task ChangeRoleAsync(RoleChangeDto roleChangeDto);
        Task EditAsync(UserDto user);

    }
}
