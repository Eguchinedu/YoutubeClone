using Microsoft.EntityFrameworkCore;
using YoutubeClone.Data;
using YoutubeClone.Interfaces;
using YoutubeClone.Models;

namespace YoutubeClone.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public  UserRepository(DataContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async  Task<bool> AddUser(UserModel user)
        {
            await _context.UserModels.AddAsync(user);

            return Save();
        }
        public async Task<bool> UserExists(int id)
        {
            return await _context.UserModels.AnyAsync(p => p.UserId == id);
        }
        public async Task<UserModel> GetUser(int id)
        {

            return await _context.UserModels.Include(p => p.Posts).Where(p => p.UserId == id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<UserModel>> GetUsers()
        {
            return await _context.UserModels.OrderBy(x => x.UserId).ToListAsync();
        }

        public bool Save()
        {
            var saved =  _context.SaveChanges();
            return   saved > 0 ? true : false;
        }


        public async Task<bool> ChangePassword(UserModel user, string newPassword)
        {
            user.Password = _passwordHasher.Hash(newPassword);
             _context.Update(user);
            return Save();
        }

        public async Task<UserModel> GetUserByName(string userName)
        {
            return await _context.UserModels.Where(p => p.UserName == userName).FirstOrDefaultAsync();
        }



       

      

    }
}
