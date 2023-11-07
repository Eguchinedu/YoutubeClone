using YoutubeClone.Models;

namespace YoutubeClone.Interfaces
{
    public interface IUserRepository
    {
        ICollection<UserModel> GetUsers();

        UserModel GetUser(int id);

        UserModel GetUserByName(string userName);

        bool UserExists(int id);

        bool AddUser(UserModel user);

        bool ChangePassword(UserModel user, string Password);

        bool Save();
    }
}
