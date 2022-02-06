using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Yello.Core.DTOs;
using Yello.Core.DTOs.Auth;
using Yello.Core.DTOs.Course;
using Yello.Core.Models;
using Yello.Core.DTOs.User;
using Yello.Keycloak.Models;

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

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            CreateMap<JsonPatchDocument<User>, JsonPatchDocument<UserProfileDto>>().ReverseMap();
            CreateMap<Operation<User>, Operation<UserProfileDto>>().ReverseMap();



        }
    }
    
}
