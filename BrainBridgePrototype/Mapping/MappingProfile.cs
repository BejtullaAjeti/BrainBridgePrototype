using AutoMapper;
using BrainBridgePrototype.DTOs;
using BrainBridgePrototype.Models;

namespace BrainBridgePrototype.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<PostDto, Post>().ReverseMap();
            CreateMap<CommentDto, Comment>().ReverseMap();
        }
    }
}
