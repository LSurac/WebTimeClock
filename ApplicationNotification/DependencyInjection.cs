using ApplicationNotification.Configurations;
using ApplicationNotification.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationNotification
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationNotification(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions<SmtpSettings>()
                .Bind(configuration
                    .GetSection(SmtpSettings.SectionName)
                );

            services.AddOptions<NotificationSettings>()
                .Bind(configuration
                    .GetSection(NotificationSettings.SectionName)
                );

            services.AddTransient<EmailService>();

            return services;
        }
    }
}
