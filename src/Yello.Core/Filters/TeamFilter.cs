using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yello.Core.Filters
{
    public class TeamFilter : BaseFilter
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
