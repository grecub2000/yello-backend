using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.DTOs.User;
using Yello.Core.DTOs.Card;

namespace Yello.Core.DTOs.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public UserDto User { get; set; }
        //public virtual CardDto Card { get; set; }
        //public virtual UserDto User { get; set; }
    }
}
