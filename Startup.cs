using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoListWebApp.Context;
using ToDoListWebApp.Services;

namespace ToDoListWebApp
{
    public class Startup
    {
	    public Startup(IConfiguration configuration)
	    {
		    Configuration = configuration;
	    }

	    public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ToDoListContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
	        
	        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		        .AddCookie(options => //CookieAuthenticationOptions
		        {
			        options.LoginPath = new PathString("/Account/Login");
		        });

            services.AddMvc();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<ITimerService, TimerService>();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();    
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
	            endpoints.MapControllerRoute(
		            name: "default",
		            pattern: "{controller=Index}/{action=Index}/{id?}");
            });
        }
    }
}
