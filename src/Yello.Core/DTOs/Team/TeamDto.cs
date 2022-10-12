using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.DTOs.User;
using Yello.Core.DTOs.Project;


namespace Yello.Core.DTOs.Team
{
    public class TeamDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int ManagerId { get; set; }
        public string Description { get; set; }
        public virtual UserDto Manager { get; set; }
        public virtual ICollection<UserDto> Users { get; set; }
        public virtual ICollection<ProjectDto> Projects { get; set; }


    }
}
