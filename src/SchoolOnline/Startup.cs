using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;

namespace SchoolOnline
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
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"E:\StaticFiles")),
                RequestPath = new PathString("")
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "diy",
                    template: "DIY/{id}.html",
                    defaults: new { controller = "Home", action = "DIY" },
                    constraints: new { id = new IntRouteConstraint() });
                routes.MapRoute(
                    name: "questionlist",
                    template: "QuestionList/{type}/{page}.html",
                    defaults: new { controller = "Home", action = "QuestionList" },
                    constraints: new { page = new IntRouteConstraint() });
                routes.MapRoute(
                    name: "question",
                    template: "Question/{rootId}/{pageId}.html",
                    defaults: new { controller = "Home", action = "Question" },
                    constraints: new { pageId = new IntRouteConstraint() });
                routes.MapRoute(
                    name: "us_english_products",
                    template: "{type}/{id}.html",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { id = new IntRouteConstraint() });
                routes.MapRoute(
                    name: "index",
                    template: "{action}.html",
                    defaults: new { controller = "SuperPage" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=SuperPage}/{action=Index}");
            });
        }
    }
}
