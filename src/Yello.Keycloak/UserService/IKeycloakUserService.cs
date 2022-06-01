using Yello.Keycloak.Models;

namespace Yello.Keycloak.UserService
{
    public interface IKeycloakUserService
    {
        Task<KeycloakResponse> LoginAsync(LoginDto loginDto);
        Task<KeycloakResponse> RegisterAsync(RegisterDto registerDto);
        Task AddRoleToUserAsync(string userId, RoleDto role);
        Task RemoveRoleFromUserAsync(string userId, RoleDto role);
    }
}