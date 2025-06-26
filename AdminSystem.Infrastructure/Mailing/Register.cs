using Core.Application.Common.Mailing;

namespace CFEMS.Infrastructure.Mailing
{
    public static class Register
    {
        public static void AddMailing(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)))
                .AddTransient<IMailService, MailService>();
        }
    }
}
