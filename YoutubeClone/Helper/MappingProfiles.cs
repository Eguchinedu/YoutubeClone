using AutoMapper;
using YoutubeClone.Dtos;
using YoutubeClone.Models;

namespace YoutubeClone.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserModel, UserDto>();

            CreateMap<UserDto, UserModel>();

            CreateMap<UserModel, UserForCreationDto>();

            CreateMap<UserForCreationDto, UserModel>();


            //Posts Mapper

            CreateMap<PostModel, PostDto>();

            CreateMap<PostDto, PostModel>();

            CreateMap<PostModel, PostForCreationDto>();

            CreateMap<PostForCreationDto, PostModel>();

            //Comment mapper

            CreateMap<CommentModel, CommentForCreationDto>();

            CreateMap<CommentForCreationDto, CommentModel>();

            CreateMap<CommentModel, CommentDto>();

            CreateMap<CommentDto, CommentModel>();
        }
    }
}
