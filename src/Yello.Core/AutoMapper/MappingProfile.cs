using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Yello.Core.DTOs;
using Yello.Core.DTOs.Auth;
using Yello.Core.Models;
using Yello.Core.DTOs.User;
using Yello.Core.DTOs.Team;
using Yello.Core.DTOs.Project;
using Yello.Core.DTOs.Sprint;
using Yello.Core.DTOs.Card;
using Yello.Core.DTOs.Comment;
using Yello.Core.DTOs.Comment;
using Yello.Keycloak.Models;
using Yello.Core.Filters; 

namespace Yello.Core.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {                                                                                                                                                                                                                                                                       
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserProfileDto>().ReverseMap();
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, RegisterDto>().ReverseMap();
            CreateMap<User, LoginResponseDto>().ReverseMap();

            CreateMap<UserRegisterDto, RegisterDto>().ReverseMap();

            CreateMap<User, UserFilter>().ReverseMap();

            CreateMap<Team, TeamDto>().ReverseMap();
            CreateMap<Team, TeamCreateDto>().ReverseMap();

            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Project, ProjectCreateDto>().ReverseMap();

            CreateMap<Sprint, SprintDto>().ReverseMap();
            CreateMap<Sprint, SprintCreateDto>().ReverseMap();
            CreateMap<Sprint, SprintCompleteDto>().ReverseMap();
            CreateMap<Sprint, SprintInfoDto>().ReverseMap();



            CreateMap<Card, CardDto>().ReverseMap();
            CreateMap<Card, CardCreateDto>().ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CommentCreateDto>().ReverseMap();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            CreateMap<JsonPatchDocument<User>, JsonPatchDocument<UserProfileDto>>().ReverseMap();
            CreateMap<Operation<User>, Operation<UserProfileDto>>().ReverseMap();



        }
    }
    
}
