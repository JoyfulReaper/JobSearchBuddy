using JobSearchBuddy.Server.Contacts;
using JobSearchBuddy.Server.Contacts.Interfaces;
using JobSearchBuddy.Server.Data;
using JobSearchBuddy.Server.Data.Interfaces;
using JobSearchBuddy.Server.Jobs;
using JobSearchBuddy.Server.Jobs.Interfaces;

namespace JobSearchBuddy.Server;

public static class DependencyInjection
{
    public static IServiceCollection AddJobSearchBuddy(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IContactRepository, ContactRepository>();
        services.AddTransient<IJobRepository, JobRepository>();
        services.AddTransient<IDbConnectionFactory, SqlConnectionFactory>();

        return services;
    }
}