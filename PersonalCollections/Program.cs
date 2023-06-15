using Application;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(IdentityConstants.ExternalScheme)
	.AddCookie(options => {
		options.LoginPath = new PathString("/Identity/GoogleSignin");
		//options.Events.OnValidatePrincipal += ActiveUserValidator.ValidateAsync;
	})
	.AddGoogle(options => {
		options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
		options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
	});
//builder.Services.Configure<SecurityStampValidatorOptions>(options => {
//	options.ValidationInterval = TimeSpan.FromSeconds(1);
//});
builder.Services.ConfigureApplicationCookie(options => {
	options.LoginPath = new PathString("/Identity/GoogleSignin");
});

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

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
