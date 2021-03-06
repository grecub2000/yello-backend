using Yello.Core.Middlewares.ExceptionMiddleware;
using Yello.Infrastructure.Seeders;
using Yello.KeyCloak;
using Yello.KeyCloak.AuthorizationHandler;
using Microsoft.AspNetCore.Authorization;

namespace Yello.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMapper();

            services.AddDatabaseContext(Configuration);

            //// KEYCLOAK
            services.AddKeycloak(Configuration);

            services.AddControllers();
            services.AddSwagger();
            services.AddServices();
            services.AddTransient<InitialSeeder>();
            services.AddMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, InitialSeeder seeder)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseStaticFiles();
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c => { });
                app.UseSwaggerUI(
                    c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Yello.Api v1");
                        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                        c.InjectStylesheet("/Content/SwaggerDark.css");
                        c.OAuthClientId("courserio");
                        c.OAuthAppName("Yello");
                        c.OAuthScopeSeparator(" ");
                    });
            }

           

            app.UseCors("allowAll");

           

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            seeder.Seed();
        }
    }
}
