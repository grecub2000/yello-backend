using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yello.Core.Models
{
    public class Project
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Team Team { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Sprint> Sprints { get; set; }
    }
}
