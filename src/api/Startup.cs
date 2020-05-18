using api.core.auth.jwt;
using api.core.auth.managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace api
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
            services.AddControllers();

            // symmetric
            // services.Configure<SymmetricOptions>(Configuration.GetSection("Jwt:Symmetric"));
            // services.AddSingleton<IJwtManager, SymmetricJwtManager>();
            
            // asymmetric
            services.Configure<AsymmetricOptions>(Configuration.GetSection("Jwt:Asymmetric"));
            services.AddSingleton<IJwtManager, AsymmetricJwtManager>();

            // jwt validation options, we do not need different implementation for this one
            var jwtOptions = new JwtValidationOptions();
            Configuration.GetSection("JwtValidation").Bind(jwtOptions);

            // setup jwt bearer authentication service            
            services.AddJwtBearerAuthentication(jwtOptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
