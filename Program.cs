
using Intern2Grow_Auth.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace Intern2Grow_Auth
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddIdentity<User, IdentityRole>(o => {
				o.Password.RequireNonAlphanumeric = false;
				o.Password.RequireDigit = false;
				o.Password.RequireUppercase = false;
				o.Password.RequireLowercase = false;
			}).AddEntityFrameworkStores<AppDbContext>();

			builder.Services.AddSession(o =>
			{
				o.IdleTimeout = TimeSpan.FromMinutes(30);
			});

			builder.Configuration.AddJsonFile("appsettings.json");

			builder.Services.AddDbContext<AppDbContext>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();
			app.UseSession();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}