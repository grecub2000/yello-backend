using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.Enums;


namespace Yello.Core.DTOs.Card
{
    public class CardCreateDto
    {
        //public int Id { get; set; }
        public CardTypeEnum Type { get; set; }
        public CardPriorityEnum Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId { get; set; }
        public int ReporterId { get; set; }
        public int SprintId { get; set; }
        //public DateTime Created { get; set; }

        //public virtual UserDto Assignee { get; set; }
        //public virtual UserDto Reporter { get; set; }

        //public virtual ICollection<SprintDto> Sprints { get; set; }
    }
}
