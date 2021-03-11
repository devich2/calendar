using System;
using System.IO;
using System.Reflection;
using Calendar.BLL;
using Calendar.DAL;
using Calendar.Web.Infrastructure.Extension;
using Calendar.Web.Infrastructure.Middleware;
using Calendar.Web.Infrastructure.SwaggerConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

namespace Calendar.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Add(new StatusCodeConvention());
                options.Filters.Add(new ModelStateValidationFilter());
            }).AddNewtonsoftJson(options => { options.SerializerSettings.Converters.Add(new StringEnumConverter()); });
            
            BlDependencyInstaller.Install(services);
            DalDependencyInstaller.Install(services, Configuration);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Calendar.Web", Version = "v1"});
                c.DocumentFilter<HideDocsFilter>();
                c.GeneratePolymorphicSchemas();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddMvc();
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation("Applying migrations");

                using (var scope = app.ApplicationServices.CreateScope())
                {
                    
                    var context = scope.ServiceProvider.GetService<CalendarDbContext>();
                    if (context != null) context.Database.Migrate();
                }
                
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            
            app.UseRouting();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calendar.Web v1");
            });
            
            app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "index_default",
                    pattern: "{controller=Home}/{action=Index}"
                );
                endpoints.MapRazorPages();
            });
            
        }
    }
}