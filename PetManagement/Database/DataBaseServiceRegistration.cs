using Microsoft.EntityFrameworkCore;
using PetManagement.Database.Context;
using PetManagement.Database.Repositories.ActivityRepository;
using PetManagement.Database.Repositories.FoodRepository;
using PetManagement.Database.Repositories.HealthStatusRepository;
using PetManagement.Database.Repositories.PetRepository;
using PetManagement.Database.Repositories.SocialInteractionRepository;
using PetManagement.Database.Repositories.TrainingRepository;
using PetManagement.Database.Repositories.UserRepository;

namespace PetManagement.Database;

public static class DataBaseServiceRegistration
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PetManagementDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("PetManagement")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPetRepository, PetRepository>();
        services.AddScoped<IHealthStatusRepository, HealthStatusRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IFoodRepository, FoodRepository>();
        services.AddScoped<ISocialInteractionRepository, SocialInteractionRepository>();
        services.AddScoped<ITrainingRepository, TrainingRepository>();

        return services;

    }
}
