using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yello.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }

        public virtual Card Card { get; set; }
        public virtual User User { get; set; }
    }
}
