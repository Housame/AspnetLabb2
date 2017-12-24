using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RolesAndClaims.Entities;

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, UserRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();
            services.AddMvc();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("HiddenNews", policy => policy.RequireRole("Publisher", "Subscriber", "Administrator"));
                options.AddPolicy("AgeRequirement", policy => policy.RequireClaim("News", "Adult", "Admin", "AllPublisher"));
                options.AddPolicy("SportsRequirement", policy => policy.RequireClaim("News", "PublishSport", "Admin"));
                options.AddPolicy("PublishEconomy", policy => policy.RequireClaim("News", "PublishEconomy", "Admin"));
                options.AddPolicy("CultureRequirement", policy =>policy.RequireClaim("News", "PublishCulture", "Admin"));
            });
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new List<string> { "index.html" }
            });
            app.UseStaticFiles();
            app.UseDirectoryBrowser();
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseMvc();



        }
    }
}
