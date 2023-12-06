using YoutubeClone.Models;

namespace YoutubeClone.Interfaces
{
    public interface IUserRepository
    {
        Task <ICollection<UserModel>> GetUsers();

        Task<UserModel> GetUser(int id);

        Task<UserModel> GetUserByName(string userName);

        Task<bool> UserExists(int id);

        Task <bool> AddUser(UserModel user);

        Task <bool> ChangePassword(UserModel user, string Password);

        bool Save();
    }
}
