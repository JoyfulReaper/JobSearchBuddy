using JobSearchBuddy.Server.Contacts;
using JobSearchBuddy.Server.Contacts.Interfaces;
using JobSearchBuddy.Server.Data;
using JobSearchBuddy.Server.Data.Interfaces;

namespace JobSearchBuddy.Server;

public static class DependencyInjection
{
    public static IServiceCollection AddJobSearchBuddy(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IContactRepository, ContactRepository>();
        services.AddTransient<IDbConnectionFactory, SqlConnectionFactory>();

        return services;
    }
}