using YoutubeClone.Data;
using YoutubeClone.Models;

namespace YoutubeClone
{
    public class Seed
    {
        private readonly DataContext _dataContext;

        public Seed(DataContext context)
        {
            _dataContext = context;
        }

        public void SeedDataContext()
        {
            if (!_dataContext.UserModels.Any())
            {
                var users = new List<UserModel>()
                {
                new UserModel()
                {
                    FirstName = "Egu",
                    LastName = "Chinedu",
                    UserName = "Eguchinedu",
                    Password = "password123",
                    Posts = new List<PostModel>()
                    {
                        new PostModel()
                        {
                            VideoUrl = "https://youtu.be/EmV_IBYIlyo?si=63cbLrSTtRVP-JsD",
                            Likes = 6,
                            Comments = new List<CommentModel>()
                            {
                                new CommentModel()
                                {
                                    Comment =  "Love it!!"
                                }
                            }
                            
                        }
                    }
                },
                new UserModel()
                {
                    FirstName = "Zeus",
                    LastName = "egu",
                    UserName = "Zeus_egu",
                    Password = "password123",
                    Posts = new List<PostModel>()
                    {
                        new PostModel()
                        {
                            VideoUrl = "https://youtu.be/EmV_IBYIlyo?si=63cbLrSTtRVP-JsD",
                            Likes = 8,
                            Comments = new List<CommentModel>()
                            {
                                new CommentModel()
                                {
                                    Comment =  "Amazing video"
                                },
                                new CommentModel()
                                {
                                    Comment = "So well taught!!"
                                }
                            }

                        }
                    }
                }
                 };
                _dataContext.UserModels.AddRange(users);
                _dataContext.SaveChanges();
            }
        }
    }
}
