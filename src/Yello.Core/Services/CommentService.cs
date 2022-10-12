using Yello.Core.Models;
using Yello.Core.DTOs;
using Yello.Core.Interfaces.Services;
using Yello.Core.Interfaces.Repositories;
using Yello.Keycloak.UserService;
using AutoMapper;
using Yello.Core.Constants;
using Yello.Core.DTOs.Auth;
using Yello.Core.DTOs.User;
using Yello.Keycloak.Models;
using Microsoft.EntityFrameworkCore;
using Yello.Core.Filters;
using Yello.Core.Middlewares.ExceptionMiddleware.CustomExceptions;
using Yello.Pagination;
using Newtonsoft.Json.Linq;
using Yello.Core.DTOs.Comment;

namespace Yello.Core.Services
{
    public class CommentService : ICommentService
    {

        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IGenericRepository<Team> _teamRepository;
        private readonly IMapper _mapper;

        public CommentService(IGenericRepository<User> userRepository, IGenericRepository<Comment> commentRepository, IGenericRepository<Team> teamRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _teamRepository = teamRepository;

            _mapper = mapper;
            //_jwtHandler = new JwtSecurityTokenHandler();
        }

        public async Task<CommentDto> GetByIdAsync(int id)
        {
            var comment = await _commentRepository
                .AsQueryable()
                //.Include(x => x.Users)
                //.Include(x => x.Team)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<CommentDto>(comment);
            return result;
        }


        public async Task<CommentDto> CreateAsync(CommentCreateDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
            if (comment.Created == DateTime.MinValue)
            {
                comment.Created = DateTime.Now;
            }
            await _commentRepository.AddAsync(comment);
            var user = await _userRepository
                .AsQueryable()
                .Where(x => x.Id == comment.UserId)
                .FirstOrDefaultAsync();
            var userRes = _mapper.Map<UserDto>(user);
            var result = _mapper.Map<CommentDto>(comment);
            result.User = userRes;
            return result;

        }

    }
}
