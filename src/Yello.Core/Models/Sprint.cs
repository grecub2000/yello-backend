using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yello.Core.Models
{
    public class Sprint
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Descrption { get; set; }
        public bool IsActive { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
    }
}
