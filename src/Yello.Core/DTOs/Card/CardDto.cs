using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.Enums;
using Yello.Core.DTOs.User;
using Yello.Core.DTOs.Sprint;
using Yello.Core.DTOs.Comment;


namespace Yello.Core.DTOs.Card
{
    public class CardDto
    {
        public int Id { get; set; }
        public CardTypeEnum Type { get; set; }
        public CardPriorityEnum Priority { get; set; }
        public CardProgressEnum Progress { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId { get; set; }
        public int ReporterId { get; set; }
        public int SprintId { get; set; }

        //public DateTime Created { get; set; }

        public virtual UserDto Assignee { get; set; }
        public virtual UserDto Reporter { get; set; }
        public virtual SprintInfoDto Sprint { get; set; }
        public virtual ICollection<CommentDto> Comments { get; set; }


        //public virtual ICollection<SprintDto> Sprints { get; set; }

    }
}
