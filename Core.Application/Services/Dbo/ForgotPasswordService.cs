using Core.Application.Commands.Dbo;
using Core.Application.Commands.Dbo.ChangePassword;
using Core.Application.Common.Mailing;
using Core.Application.Interfaces.Dbo.ServiceInterfaces;

namespace Core.Application.Services.Dbo
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private User user;
        private MailRequest mailRequest;
        private int otp;
        public ForgotPasswordService(IUserService userService, IMailService mailService, IConfiguration config)
        {
            _userService = userService;
            _mailService = mailService;
            _config = config;
        }

        public async Task<ForgotPasswordService> CheckUserWithEmail(string email)
        {
            var userFromDb = await _userService.GetByEmailAsync(email);
            if (userFromDb == null) throw new InvalidDataException("Invalid Email");
            user = userFromDb;

            return this;
        }

        public async Task<ForgotPasswordService> GenerateOtp()
        {
            otp = GenerateRandomNo();
            string formatedOtp = otp.ToString().PadLeft(6, '0');

            user = new User()
            {
                ID = user.ID,
                OTP = Convert.ToInt32(formatedOtp),
                EMAIL = user.EMAIL,
                OTP_EXPIRY_TIMIE = DateTime.Now.AddMinutes(double.Parse(_config["SecuritySettings:OtpSettings:otpExpirationInMinutes"]))
            };

            mailRequest = new MailRequest()
            {
                To = user.EMAIL,
                Subject = "OTP for verification",
                Body = string.Format("Hello user, Your OTP is : " + user.OTP + "  Thank You.")
            };

            await _userService.UpdateUserOtp(user);

            return this;
        }
        public static int GenerateRandomNo()
        {
            int _min = 100000;
            int _max = 999999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public async Task<bool> SendEmail()
        {
            return await _mailService.SendEmail(mailRequest);
        }

        public async Task<bool> VerifyOtp(int code, string email)
        {
            var otpWithExpiry = await _userService.VerifyOtp(code, email);
            if (otpWithExpiry == null) throw new InvalidDataException("Invalid OTP");

            var otpExpiryTime = otpWithExpiry.OTP_EXPIRY_TIMIE;
            if (otpExpiryTime < DateTime.Now) throw new UnauthorizedAccessException("Otp is expired");
            return true;
        }


        public async Task<int> ChangePassword(ChangePasswordCommand model)
        {
            if (!string.IsNullOrEmpty(model.CurrentPassword))
            {
                var userFromDb = await _userService.GetByNameAsync(model.UserName);

                if (userFromDb == null) throw new UnauthorizedAccessException("Invalid User Email");

                using var hmac = new HMACSHA512(Convert.FromBase64String(userFromDb.PASSWORD_SALT));
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.CurrentPassword));

                if (Convert.ToBase64String(computedHash) != userFromDb.PASSWORD) throw new InvalidDataException("Invalid Current Password");

                var userWithNewPassword = GenerateNewPasswordForUser(model.NewPassword, model.UserName);
                return await _userService.ChangePassword(userWithNewPassword);

            }
            else
            {
                var userPassword = GenerateNewPasswordForUser(model.NewPassword, model.UserName);
                return await _userService.ChangePassword(userPassword);
            }



        }

        private User GenerateNewPasswordForUser(string newPassword, string userName)
        {
            var hmac = new HMACSHA512();
            var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));

            return new User
            {
                USER_NAME = userName,
                PASSWORD = Convert.ToBase64String(computerHash),
                PASSWORD_SALT = Convert.ToBase64String(hmac.Key)
            };
        }
    }
}
