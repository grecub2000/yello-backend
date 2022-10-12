using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.DTOs.Team;
using Yello.Core.DTOs.User;
using Yello.Core.DTOs.Sprint;

namespace Yello.Core.DTOs.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //public virtual TeamDto Team { get; set; }
        public virtual ICollection<UserDto> Users { get; set; }
        public virtual ICollection<SprintDto> Sprints { get; set; }
    }
}
