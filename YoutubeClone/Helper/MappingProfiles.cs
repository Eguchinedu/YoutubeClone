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

            CreateMap<UserModel, UserWithPostDto>();

            CreateMap<UserWithPostDto, UserModel>();


            //Posts Mapper

            CreateMap<PostModel, PostDto>();

            CreateMap<PostDto, PostModel>();

            CreateMap<PostModel, PostForCreationDto>();

            CreateMap<PostForCreationDto, PostModel>();

            CreateMap<PostForDisplayDto, PostModel>();

            CreateMap<PostModel, PostForDisplayDto>();

            CreateMap<PostForDisplayDto, PostForDisplayDto>();

            CreateMap<PostLike, UserLikeDto>();

            CreateMap<UserLikeDto, PostLike>();

            //Comment mapper

            CreateMap<CommentModel, CommentForCreationDto>();

            CreateMap<CommentForCreationDto, CommentModel>();


            CreateMap<CommentModel, CommentDto>();

            CreateMap<CommentDto, CommentModel>();
        }
    }
}
