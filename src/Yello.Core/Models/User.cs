using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Yello.Core.Enums;

namespace Yello.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string KeycloakId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Url]
        public string ProfilePicture { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }


        // Navigation Properties
        public virtual Role Role { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
        [InverseProperty("Manager")]
        public virtual ICollection<Team> TeamsManager { get; set; }  
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty("Assignee")]
        public virtual ICollection<Card> AssigneeCards { get; set; }
        [InverseProperty("Reporter")]
        public virtual ICollection<Card> ReporterCards { get; set; }
    }
}
