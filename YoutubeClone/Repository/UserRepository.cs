using YoutubeClone.Data;
using YoutubeClone.Interfaces;
using YoutubeClone.Models;

namespace YoutubeClone.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(DataContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public bool AddUser(UserModel user)
        {
            _context.UserModels.Add(user);

            return Save();
        }
        public bool UserExists(int id)
        {
            return _context.UserModels.Any(p => p.UserId == id);
        }
        public UserModel GetUser(int id)
        {

            return _context.UserModels.Where(p => p.UserId == id).FirstOrDefault();
        }

        public ICollection<UserModel> GetUsers()
        {
            return _context.UserModels.OrderBy(x => x.UserId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }


        public bool ChangePassword(UserModel user, string newPassword)
        {
            user.Password = _passwordHasher.Hash(newPassword);
            _context.Update(user);
            return Save();
        }

        public UserModel GetUserByName(string userName)
        {
            return _context.UserModels.Where(p => p.UserName == userName).FirstOrDefault();
        }



       

      

    }
}
