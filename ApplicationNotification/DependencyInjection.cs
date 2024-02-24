using ApplicationNotification.Configurations;
using ApplicationNotification.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ApplicationNotification
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationNotification(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            services.AddSingleton(provider => provider.GetRequiredService<IConfiguration>().GetSection(NotificationSettings.SectionName).Get<NotificationSettings>());
            services.AddSingleton(provider => provider.GetRequiredService<IConfiguration>().GetSection(SmtpSettings.SectionName).Get<SmtpSettings>());

            services.AddTransient<EmailService>();

            return services;
        }
    }
}
