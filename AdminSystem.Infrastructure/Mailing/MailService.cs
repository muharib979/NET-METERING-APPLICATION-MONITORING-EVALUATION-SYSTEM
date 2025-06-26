using Core.Application.Common.Mailing;

using MailKit.Net.Smtp;
using MailKit.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;


namespace CFEMS.Infrastructure.Mailing
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;
        private readonly ILogger<MailService> _logger;

        public MailService(IOptions<MailSettings> settings, ILogger<MailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmail(MailRequest request)
        {
            try
            {
                var email = new MimeMessage();

                // From
                email.From.Add(new MailboxAddress(_settings.DisplayName, request.From ?? _settings.From));

                // To
                email.To.Add(MailboxAddress.Parse(request.To));

                // Content
                var builder = new BodyBuilder();
                email.Sender = new MailboxAddress(request.DisplayName ?? _settings.DisplayName, request.From ?? _settings.From);
                email.Subject = request.Subject;
                builder.HtmlBody = request.Body;

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
