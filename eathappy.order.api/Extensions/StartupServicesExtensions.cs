using eathappy.order.api.Filters;
using eathappy.order.api.JwtFeatures;
using eathappy.order.business.External.DynamicPlatform.Implementations;
using eathappy.order.business.External.DynamicPlatform.Interfaces;
using eathappy.order.business.Implementations;
using eathappy.order.business.Interfaces;
using eathappy.order.business.Mappers;
using eathappy.order.business.Validator;
using eathappy.order.data.Context;
using eathappy.order.data.Interfaces;
using eathappy.order.data.Repositories;
using eathappy.order.data.UnitOfWork;
using eathappy.order.domain.Dtos.Local.Parameter;
using eathappy.order.domain.User;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace eathappy.order.api.Extensions
{
#pragma warning disable CS1591 // Unrecognized #pragma directive
    public static class StartupServicesExtensions
    {
        public static void AddApplicationHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStringOptions = configuration["ApplicationSettings:ConnectionStringSql"];

            services.AddHealthChecks()
               .AddCheck("live", () => HealthCheckResult.Healthy(), new[] { "live" })
               .AddSqlServer(connectionStringOptions, tags: new[] { "ready" });
        }

        public static void RegisterSwaggerForm(this IServiceCollection services)
        {
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }

        public static void RegisterCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .WithOrigins("*")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination"));
            });
        }

        public static void AddControllersWithConfiguredSerialization(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(2, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }

        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EatHappy Orders API",
                    Version = configuration.GetValue("SERVICE_VERSION", "v1"),
                    Description = "EatHappy Orders API swagger page"
                });
                options.OperationFilter<SwaggerFileOperationFilter>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                        },
                        Array.Empty<string>()
                    }
                });

                var basePath = AppContext.BaseDirectory;
                var xmlPathWeb = Path.Combine(basePath, "eathappy.order.api.xml");
                options.IncludeXmlComments(xmlPathWeb);
            });
        }

        public static void RegisterApplicationLayers(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDbContext(configuration);
            services.RegisterRepositories();
            services.RegisterApplicationServices();
            services.AddHttpContextAccessor();
            services.RegisterMappers();
            services.ConfigureIdentity();
            services.ConfigureAuthentication(configuration);
            services.RegisterJwtHandler();
            services.RegisterCAP(configuration);
            services.RegisterValidators();
        }

        private static void RegisterValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<OrderDto>, OrderDtoValidator>();
            services.AddScoped<IValidator<ArticleDto>, ArticleDtoValidator>();

        }

        public static void RegisterCAP(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCap(capOptions =>
            {
                capOptions.UseEntityFramework<DatabaseContext>();
                capOptions.UseNATS(natsOptions =>
                {
                    natsOptions.Servers = configuration["ApplicationSettings:CAPUrl"].ToString();
                });
            });
        }

        private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value))
                };
            });
        }

        private static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DatabaseContext>();
        }

        private static void RegisterJwtHandler(this IServiceCollection services)
        {
            services.AddScoped<JwtHandler>();
        }

        private static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(
              options =>
              {
                  options.UseSqlServer(configuration["ApplicationSettings:ConnectionStringSql"], b => b.MigrationsAssembly(@"eathappy.order.data"));
              });
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        private static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IDynamicsPlatformService, DynamicsPlatformService>();
        }

        private static IServiceCollection RegisterMappers(this IServiceCollection services)
            => services.AddAutoMapper(
                typeof(MapperConfig).GetTypeInfo().Assembly);
    }
#pragma warning restore CS1591 // Unrecognized #pragma directive
}
