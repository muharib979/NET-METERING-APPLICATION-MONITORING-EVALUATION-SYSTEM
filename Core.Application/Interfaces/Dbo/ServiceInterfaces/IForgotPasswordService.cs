using Core.Application.Commands.Dbo;
using Core.Application.Commands.Dbo.ChangePassword;
using Core.Application.Common.Mailing;
using Core.Application.Services.Dbo;

namespace Core.Application.Interfaces.Dbo.ServiceInterfaces
{
    public interface IForgotPasswordService
    {
        public Task<ForgotPasswordService> CheckUserWithEmail(string email);
        public Task<ForgotPasswordService> GenerateOtp();
        public Task<bool> SendEmail();
        public Task<bool> VerifyOtp(int code, string email);
        Task<int> ChangePassword(ChangePasswordCommand model);
    }
}
