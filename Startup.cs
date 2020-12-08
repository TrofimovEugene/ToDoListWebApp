using System;
using System.Threading;
using Autofac;
using Autofac.Extensions.DependencyInjection;
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
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ToDoListWebApp
{
    public class Startup
    {
	    public Startup(IHostingEnvironment env)
	    {
		    var builder = new ConfigurationBuilder()
			    .SetBasePath(env.ContentRootPath)
			    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
			    .AddEnvironmentVariables();
		    this.Configuration = builder.Build();
	    }

	    public IConfiguration Configuration { get; }
	    public ILifetimeScope AutofacContainer { get; private set; }
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
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
        }
        
        public void ConfigureContainer(ContainerBuilder builder)
        {
	        // Register your own things directly with Autofac here. Don't
	        // call builder.Populate(), that happens in AutofacServiceProviderFactory
	        // for you.
	        builder.RegisterType<EmailService>().As<IEmailService>().SingleInstance();
	        builder.RegisterType<TimerService>().As<ITimerService>().SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
	        AutofacContainer = app.ApplicationServices.GetAutofacRoot();
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

            AutofacContainer.Resolve<ITimerService>().StartAsync(CancellationToken.None);
        }
    }
}
