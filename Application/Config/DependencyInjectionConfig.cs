using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Infrastructure.MockData;
using Infrastructure.DAL;
using Application.Repos;
using Application.Services;

namespace Application.Config 
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            // Register the SQL DB Connection Factory
            services.AddScoped<ISqlDbConnFactory, SqlDbConnFactory>();

            // Register the SQL Data Access Service
            services.AddScoped<IDataAccess, DataAccess>();

            // Register MockData
            services.AddScoped<IDataBuilderService, DataBuilderService>();
            services.AddScoped<IMockStartup, MockStartup>();

            // Register Repositories
            services.AddScoped<IAppModuleRepository, AppModuleRepository>();
            services.AddScoped<IUserModuleSelectRepository, UserModuleSelectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Register Services
            services.AddScoped<IAppModuleService, AppModuleService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
