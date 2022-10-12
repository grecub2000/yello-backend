using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.DTOs.Comment;


namespace Yello.Core.Interfaces.Services
{
    public interface ICommentService
    {
        Task <CommentDto> GetByIdAsync(int id);
        Task <CommentDto> CreateAsync(CommentCreateDto commentDto);

    }
}
