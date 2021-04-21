using FittShop.Data.Abstracts;
using FittShop.Data.SqlServer;
using FittShop.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FittShop
{
    public class Startup
    {
        private readonly IConfiguration configuration;


        public Startup(IConfiguration configuration) => this.configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            //Конфиг з appsettings.json
            var config = new ProjectConfig();
            this.configuration.Bind("Project", config);

            //функціонал в якості сервісів
            services.AddSingleton(config);
            services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            //context з бд
            string connectionString = this.configuration.GetConnectionString(this.configuration["SelectedConnection"]);
            string migrationsAssName = typeof(AppDbContext).Namespace;
            services.AddDbContext<AppDbContext>(x =>
                x.UseSqlServer(connectionString, 
                    builder => builder.MigrationsAssembly(migrationsAssName)));

            //настройка identity system
            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //authentication cookie
            services.ConfigureApplicationCookie(opts =>
            {
                opts.Cookie.Name = "FittShopAuth";
                opts.Cookie.HttpOnly = true;
                opts.LoginPath = "/account/login";
                opts.AccessDeniedPath = "/account/accessdenied";
                opts.SlidingExpiration = true;
            });

            services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminArea", policy =>
                {
                    policy.RequireRole("admin");
                });
            });

            //MVC
            services.AddControllersWithViews(x =>
            {
                x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
            }).AddSessionStateTempDataProvider();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Products}/{action=All}/{id?}");
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "{area:exists}/{controller=ServiceItems}/{action=Items}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}