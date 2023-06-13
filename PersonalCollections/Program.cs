using Application;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options => {
		options.LoginPath = new PathString("/Identity/SignIn");
		//options.Events.OnValidatePrincipal += ActiveUserValidator.ValidateAsync;
	});
//builder.Services.Configure<SecurityStampValidatorOptions>(options => {
//	options.ValidationInterval = TimeSpan.FromSeconds(1);
//});

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
} else
{
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
