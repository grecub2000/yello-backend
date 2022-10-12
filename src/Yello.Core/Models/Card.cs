using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.Enums;


namespace Yello.Core.Models
{
    public class Card
    {
        public int Id { get; set; }
        public CardTypeEnum Type { get; set; }
        public CardPriorityEnum Priority { get; set; }
        public CardProgressEnum Progress { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId   { get; set; }
        public int ReporterId { get; set; }
        public int SprintId { get;set; }

        public DateTime Created { get; set; }
        public virtual User Assignee { get; set; }
        public virtual User Reporter { get; set;  }

        public virtual Sprint Sprint { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } 
    }
}
