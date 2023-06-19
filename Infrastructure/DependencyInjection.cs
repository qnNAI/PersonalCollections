﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Contracts.Contexts;
using Application.Services;
using Domain.Entities.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure {
    public static class DependencyInjection {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
					builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddSignInManager<ApplicationSignInManager>();

			services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

			return services;
        }
    }
}
