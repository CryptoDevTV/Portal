using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portal.Shared.Model.Settings;
using Portal.Shared.Services.Notifications;

namespace Portal.Web
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

            services
                .AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/course", "kurs/{id}/{name}");
                    options.Conventions.AddPageRoute("/contact", "kontakt/{contacttype}");
                    options.Conventions.AddPageRoute("/policy", "polityka-prywatnosci");
                    options.Conventions.AddPageRoute("/partnership", "wspolpraca");
                })
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}