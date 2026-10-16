
using ModelLayer.Model;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        UserModel? GetUser(int id);
        UserModel PostUser(UserModel user);
        bool EmailExists(string email);
        UserModel? GetUserByEmail(string email);
        void SaveResetToken(UserModel user, string token, DateTime expiry);
        bool ResetPassword(int userId, string token, string passwordHash);
        void ClearResetToken(int userId);
    }
}
