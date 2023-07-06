using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Mappings;
using FluentValidation;
using Application.Common.Contracts.Services;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Application.Models.Email;
using Application.Helpers;

namespace Application
{

    public static class DependencyInjection {

        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) {
            var config = TypeAdapterConfig.GlobalSettings;
            MappingProfile.ApplyMappings();

            services.AddSingleton(config);
            services.AddScoped<IMapper, Mapper>();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ICollectionService, CollectionService>();
            services.AddScoped<IItemService, ItemService>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            var emailConfig = configuration
               .GetSection("EmailConfiguration")
               .Get<EmailConfiguration>();

            services.AddSingleton(emailConfig);

            services.AddSingleton<CollectionTypes>();

            return services;
        }
    }
}
