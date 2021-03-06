using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portal.Shared.Data;
using Portal.Shared.Data.Db;
using Portal.Shared.Model.Settings;
using Portal.Shared.Services.Notifications;

namespace Portal.Academia.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var emailSettingsSection = Configuration.GetSection("EmailSettings");
            services.Configure<EmailSettings>(emailSettingsSection);

            var emailSettings = emailSettingsSection.Get<EmailSettings>();

            services
                .AddScoped<IEmailNotification>(
                    db => new EmailNotification(
                        apiKey: emailSettings.ApiKey,
                        sender: emailSettings.Sender,
                        replyTo: emailSettings.ReplyTo,
                        host: emailSettings.Host));

            services.AddScoped<IDbAccess>(
                db => new DbAccess(Configuration.GetConnectionString("CdDb")));

            services.AddTransient<IDataRepository, DataRepository>();

            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "student/{userGuid}",
                    new { controller = "Home", action = "Student" });

                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "polityka-prywatnosci",
                    new { controller = "Home", action = "Policy" });

                endpoints.MapControllerRoute(
                    name: "lessons",
                    pattern: "lessons/{userGuid}/{courseId}/{lessonId}",
                    new { controller = "Lessons", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "modules",
                    pattern: "{controller=Modules}/{action=Show}/{userGuid}/{courseId}"
                    );

                endpoints.MapControllerRoute(
                    name: "contact",
                    pattern: "contact/{userGuid}/{contactId}",
                    new { controller = "Contact", action = "Index" });
            });
        }
    }
}