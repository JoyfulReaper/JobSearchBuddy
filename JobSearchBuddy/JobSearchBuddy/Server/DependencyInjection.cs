using JobSearchBuddy.Server.Contacts;
using JobSearchBuddy.Server.Contacts.Interfaces;

namespace JobSearchBuddy.Server;

public static class DependencyInjection
{
    public static IServiceCollection AddJobSearchBuddy(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IContactRepository, ContactRepository>();

        return services;
    }
}