using System.ComponentModel.DataAnnotations.Schema;
using Yello.Core.Enums;
using Yello.Core.DTOs.Team;
using Yello.Core.DTOs.Project;
using Yello.Core.DTOs.Card;





namespace Yello.Core.DTOs.User
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<TeamDto> Teams { get; set; }
        [InverseProperty("Manager")]
        public virtual ICollection<TeamDto> TeamsManager { get; set; }
        public virtual ICollection<ProjectDto> Projects { get; set; }
        //public virtual ICollection<CommentDto> Comments { get; set; }
        [InverseProperty("Assignee")]
        public virtual ICollection<CardDto> AssigneeCards { get; set; }
        [InverseProperty("Reporter")]
        public virtual ICollection<CardDto> ReporterCards { get; set; }
    }
}
