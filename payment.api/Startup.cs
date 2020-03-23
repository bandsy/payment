using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using payment.api.EntityFramework;

namespace payment.api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddControllers ();

            services.AddEntityFrameworkNpgsql ();
            services.AddDbContext<PaymentApiDbContext> (options =>
                options.UseNpgsql (Environment.GetEnvironmentVariable ("DbConnectionString")));

            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new OpenApiInfo {
                    Version = "v1",
                        Title = "Payment Api",
                        Description = "Api to process payment & get payment details",
                });
            });

            services.AddCors (options => {
                options.AddPolicy ("AllowAllOrigins",
                    builder => {
                        builder.AllowAnyOrigin ()
                            .AllowAnyMethod ()
                            .AllowAnyHeader ();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseSwagger ();
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "Payment API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors ("AllowAllOrigins");

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}