using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UsersTable.Data;
using UsersTable.Models;

[assembly: HostingStartup(typeof(UsersTable.Areas.Identity.IdentityHostingStartup))]
namespace UsersTable.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<UsersTableContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UsersTableContextConnection")));

                services.AddDefaultIdentity<User>(options =>
                    options.Password = new PasswordOptions
                    {
                        RequireDigit = false,
                        RequiredLength = 1,
                        RequireLowercase = false,
                        RequireUppercase = false,
                        RequiredUniqueChars = 0,
                        RequireNonAlphanumeric = false
                    })
                            .AddEntityFrameworkStores<UsersTableContext>();

                            

                services.Configure<SecurityStampValidatorOptions>(options =>
                {
                    options.ValidationInterval = TimeSpan.Zero;
                });

                services.Configure<IdentityOptions>(opts =>
                {
                    opts.Lockout.AllowedForNewUsers = false;
                });
            });
        }
    }
}