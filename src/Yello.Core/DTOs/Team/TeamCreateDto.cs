using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.DTOs.User;

namespace Yello.Core.DTOs.Team
{
    public class TeamCreateDto
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public int ManagerId { get; set; }
    }
}
