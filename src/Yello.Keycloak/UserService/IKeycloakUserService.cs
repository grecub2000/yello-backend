using Yello.Keycloak.Models;

namespace Yello.Keycloak.UserService
{
    public interface IKeycloakUserService
    {
        Task<KeycloakResponse> Login(LoginDto loginDto);
        Task<KeycloakResponse> Register(RegisterDto registerDto);
    }
}