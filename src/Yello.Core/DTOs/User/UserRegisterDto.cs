using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Yello.Core.DTOs.User
{
    public class UserRegisterDto
    {
        [Required, MinLength(2), MaxLength(50)]
        public string Username { get; set; }
        [Required, MinLength(2), MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; }
        [Url,AllowNull]
        public string ProfilePicture { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
