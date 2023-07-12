using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Contracts.Contexts;
using Application.Common.Contracts.Services;
using Application.Services;
using Domain.Entities.Identity;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Services.CloudStorage;
using Infrastructure.Services.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure {
    public static class DependencyInjection {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env) {
			services.AddDbContext<ApplicationDbContext>(options => {
				var connectionString = env.IsDevelopment() ? configuration.GetConnectionString("DefaultConnection") : configuration["Remote_Connection_String"];
                options.UseSqlServer(connectionString,
						builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddSignInManager<ApplicationSignInManager>()
				.AddDefaultTokenProviders();

			services.Configure<DataProtectionTokenProviderOptions>(opts => {
				opts.TokenLifespan = TimeSpan.FromMinutes(30);
			});

			services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

			services.AddScoped<IEmailService, EmailService>();
			services.AddSingleton<ICloudStorageService, GoogleCloudStorageService>();

			return services;
        }
    }
}
