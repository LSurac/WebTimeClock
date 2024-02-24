using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlDataAccess.Contract.Configurations;
using SqlDataAccess.Services;

namespace SqlDataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton(provider => provider.GetRequiredService<IConfiguration>().GetSection(MsSqlDataSettings.SectionName).Get<MsSqlDataSettings>());

            services.AddTransient<SqlDataAccessor>();

            var dbServicesClassList = from t in Assembly.GetExecutingAssembly().GetTypes()
                where
                    t.IsClass && t.FullName.EndsWith("DbService")
                select t;

            foreach (var dbServiceClass in dbServicesClassList)
            {
                var dbServiceInterface = dbServiceClass.GetInterfaces()[0];
                services.AddTransient(dbServiceInterface, dbServiceClass);
            }

            return services;
        }
    }
}
