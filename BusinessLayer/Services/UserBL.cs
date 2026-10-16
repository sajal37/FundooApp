
using BusinessLayer.Interface;
using Middleware.Services.Email;
using Middleware.Services.Jwt;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using System.Security.Claims;

namespace BusinessLayer.Services
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL userRepository;
        private readonly IJwtTokenService jwtService;
        private readonly IEmailSender emailSender;

        public UserBL(
            IUserRL userRepository,
            IJwtTokenService jwtService,
            IEmailSender emailSender)
        {
            this.userRepository = userRepository;
            this.jwtService = jwtService;
            this.emailSender = emailSender;
        }

        public RegistrationResponseModel? PostUsers(RegistrationModel reg)
        {
            if (userRepository.EmailExists(reg.Email))
            {
                return null;
            }

            UserModel user = new UserModel
            {
                FirstName = reg.FirstName,
                LastName = reg.LastName,
                Email = reg.Email,
                ContactNo = reg.ContactNo,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(reg.Password)
            };

            UserModel savedUser = userRepository.PostUser(user);

            return new RegistrationResponseModel
            {
                Id = savedUser.Id,
                FirstName = savedUser.FirstName,
                LastName = savedUser.LastName,
                Email = savedUser.Email,
                ContactNo = savedUser.ContactNo
            };
        }

        public LoginResponseModel? Login(LoginModel login)
        {
            UserModel? user = userRepository.GetUserByEmail(login.Email);

            if (user == null ||
                string.IsNullOrWhiteSpace(user.PasswordHash) ||
                string.IsNullOrWhiteSpace(user.Email))
            {
                return null;
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(
                login.Password,
                user.PasswordHash);

            if (!passwordValid)
            {
                return null;
            }

            string token = jwtService.GenerateToken(user.Id, user.Email);

            return new LoginResponseModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = token
            };
        }

        public async Task<bool> ForgotPassword(ForgotPasswordModel freq)
        {
            UserModel? user = userRepository.GetUserByEmail(freq.Email);

            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                return false;
            }

            DateTime expiry = DateTime.UtcNow.AddMinutes(15);

            string token = jwtService.GenerateResetToken(
                user.Id,
                user.Email,
                expiry);

            userRepository.SaveResetToken(user, token, expiry);

            MessageModel msg = new MessageModel
            {
                Email = user.Email,
                Subject = "Request for Password Reset",
                Body = $"Here's the token for password reset: {token}"
            };

            try
            {
                await emailSender.SendEmailAsync(msg);
                return true;
            }
            catch (HttpRequestException)
            {
                userRepository.ClearResetToken(user.Id);
                throw;
            }
        }

        public bool ValidateResetToken(int userId, string token)
        {
            Claim? tokenTypeClaim;

            try
            {
                tokenTypeClaim = jwtService.ValidateToken(token);
            }
            catch (Exception)
            {
                return false;
            }

            if (tokenTypeClaim == null ||
                tokenTypeClaim.Value != "PasswordReset")
            {
                return false;
            }

            UserModel? user = userRepository.GetUser(userId);

            if (user == null || user.ResetToken != token)
            {
                return false;
            }

            if (user.TokenExpiry == null ||
                user.TokenExpiry <= DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        public bool ResetPassword(int userId, string token, string newPassword)
        {
            if (!ValidateResetToken(userId, token))
            {
                return false;
            }

            string passwordHash =
                BCrypt.Net.BCrypt.HashPassword(newPassword);

            return userRepository.ResetPassword(
                userId,
                token,
                passwordHash);
        }
    }
}
