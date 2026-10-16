
using ModelLayer.Model;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fundoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FundooController : ControllerBase
    {
        private readonly IUserBL userService;

        public FundooController(IUserBL userService)
        {
            this.userService = userService;
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            userId = 0;

            Claim? claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return false;
            }

            return int.TryParse(claim.Value, out userId);
        }

        [HttpPost("register")]
        public IActionResult PostUsers(RegistrationModel reg)
        {
            RegistrationResponseModel? response =
                userService.PostUsers(reg);

            if (response == null)
            {
                return Conflict(new
                {
                    message = "An account with this email already exists"
                });
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel login)
        {
            LoginResponseModel? response = userService.Login(login);

            if (response == null)
            {
                return Unauthorized();
            }

            return Ok(response);
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordModel freq)
        {
            try
            {
                await userService.ForgotPassword(freq);

                return Accepted(new
                {
                    message = "If the account exists, password-reset instructions will be sent."
                });
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, new
                {
                    message = "The email service is temporarily unavailable."
                });
            }
        }

        [HttpPost("resetpassword")]
        [Authorize(Policy = "PasswordReset")]
        public IActionResult ResetPassword(ResetPasswordModel res)
        {
            if (!TryGetCurrentUserId(out int userId))
            {
                return Unauthorized();
            }

            string authorization =
                Request.Headers["Authorization"].ToString();

            if (!authorization.StartsWith(
                "Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized();
            }

            string token =
                authorization.Substring("Bearer ".Length).Trim();

            bool result = userService.ResetPassword(
                userId, token, res.Password);

            if (!result)
            {
                return Unauthorized("Invalid or expired reset token");
            }

            return Ok("Password reset successfully");
        }
    }
}
