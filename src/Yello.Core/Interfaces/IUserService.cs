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
        Task RegisterAsync(UserRegisterDto userRegisterDto, string keycloakId);
        Task<List<UserProfileDto>> ListAsync(UserFilter userFilter);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task ChangeRoleAsync(RoleChangeDto roleChangeDto);
    }
}
