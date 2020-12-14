using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NomadAPI.Data;
using NomadAPI.Helpers;
using NomadAPI.Interfaces;
using NomadAPI.Services;
using NomadAPI.SignalR;

namespace NomadAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<PresenceTracker>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IMessageRepository, MessageRepository>();
            //services.AddScoped<IReactionsRepository, ReactionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AutomapperProfiles).Assembly);
            services.AddScoped<LogUserActivity>();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
