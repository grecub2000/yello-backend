using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.DTOs.Team;

namespace Yello.Core.DTOs.Project
{ 
    public class ProjectCreateDto
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //public virtual TeamDto Team { get; set; }

    }
}
