using System;
using System.Text;
using Boilerplate.Business.Abstract;
using Boilerplate.Business.Concrete;
using Boilerplate.Business.Core;
using Boilerplate.Core.Helpers;
using Boilerplate.DAL.Abstract;
using Boilerplate.DAL.Concrete;
using Boilerplate.DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Boilerplate
{
    public class Startup
    {
        private const string CorsAll = "all";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddDbContext<BoilerplateContext>
            (options => options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            //services.AddScoped<BlobStorageService>();

            services.AddScoped<Token>();

            services.AddScoped<ValidateModelAttribute>();

            //services.AddTransient<RedisCacheService>();

            //services.AddMemoryCache();

            services.AddCors(options =>
            {
                options.AddPolicy(CorsAll,
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });

            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddDataProtection();

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        //ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                        //ClockSkew = TimeSpan.FromMinutes(30)
                    };
                });

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = false; });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Boilerplate",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "Bugra Durukan",
                        Email = "bugray34@gmail.com"
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then user token in the text input below.\r\n\r\n Bearer yazýp boþluktan sonra tokeni giriniz\r\n\r\nExample: \"Bearer 12345abcdef\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".obj"] = "application/octet-stream";

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Boilerplate v1"));

            app.UseMiddleware(typeof(ErrorHandler));

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCors(CorsAll);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<Jwt>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}