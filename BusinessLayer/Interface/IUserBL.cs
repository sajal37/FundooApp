
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        RegistrationResponseModel? PostUsers(RegistrationModel reg);
        LoginResponseModel? Login(LoginModel login);
        Task<bool> ForgotPassword(ForgotPasswordModel freq);
        bool ValidateResetToken(int userId, string token);
        bool ResetPassword(int userId, string token, string newPassword);
    }
}
