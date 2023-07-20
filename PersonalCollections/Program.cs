using Application;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using PersonalCollections.Middleware;
using PersonalCollections.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

var port = Environment.GetEnvironmentVariable("PORT");
//builder.WebHost.UseUrls("http://*:" + port);

builder.Services
    .AddAuthentication()
	.AddCookie(options => {
		options.LoginPath = new PathString("/Identity/SignIn");
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
	options.LoginPath = new PathString("/Identity/SignIn");
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

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new[] {
        "en-US",
        "ru",
    };
    options.SetDefaultCulture(cultures[0])
        .AddSupportedCultures(cultures)
        .AddSupportedUICultures(cultures);
});

builder.Services.AddSignalR(op => {
    op.EnableDetailedErrors = true;
});

builder.Services
    .AddControllersWithViews(o => {
        o.ModelValidatorProviders.Clear();
        o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

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
var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseUserStatusValidation();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ItemHub>("/item");

app.Run();
