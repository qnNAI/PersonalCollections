using Application;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using PersonalCollections.CookieValidators;
using PersonalCollections.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddAuthentication()
	.AddCookie(options => {
		options.LoginPath = new PathString("/Identity/GoogleSignin");
		options.AccessDeniedPath = new PathString("/Identity/AccessDenied");
	})
	.AddGoogle(options => {
		options.SignInScheme = IdentityConstants.ExternalScheme;
		options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
	.AddGitHub(options => {
		options.SignInScheme = IdentityConstants.ExternalScheme;
        options.ClientId = builder.Configuration["Authentication:Github:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Github:ClientSecret"];
    });
builder.Services.ConfigureApplicationCookie(options => {
	options.AccessDeniedPath = new PathString("/Identity/AccessDenied");
});

builder.Services.AddAuthorization();

builder.Services.Configure<IdentityOptions>(opts => {
	opts.Password.RequireNonAlphanumeric = false;
	opts.Password.RequireLowercase = false;
	opts.Password.RequireUppercase = false;
	opts.Password.RequireDigit = false;
	opts.Password.RequiredLength = 1;
	
	opts.User.RequireUniqueEmail = true;
});

builder.Services.AddControllersWithViews(o => {
	o.ModelValidatorProviders.Clear();
	o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

app.UseCookiePolicy(new CookiePolicyOptions {
	MinimumSameSitePolicy = SameSiteMode.Lax
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
} else
{
	app.UseDeveloperExceptionPage();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.UseUserStatusValidation();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
