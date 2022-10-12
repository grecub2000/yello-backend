using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Yello.Core.DTOs.User;
using Yello.Core.DTOs.Project;
using Yello.Core.DTOs.Card;


namespace Yello.Core.DTOs.Sprint
{
    public class SprintCreateDto
    {
        //public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Descrption { get; set; }
        //public virtual ProjectDto Project { get; set; }
        //public virtual ICollection<CardDto> Cards { get; set; }

    }
}
