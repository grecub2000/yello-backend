﻿using AutoMapper;
using Yello.Core.AutoMapper;
using Yello.Core.Interfaces;
using Yello.Core.Interfaces.Services;
using Yello.Core.Interfaces.Repositories;
using Yello.Core.Services;
using Yello.Infrastructure.Context;
using Yello.Infrastructure.Repositories;
using Yello.Infrastructure.Seeders;
using Yello.Keycloak.UserService;
using Yello.KeyCloak;
using Yello.KeyCloak.AuthorizationHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Yello.Api
{
    public static class ServiceExtension
    {
        public static void AddKeycloak(this IServiceCollection services, IConfiguration configuration)
        {
            // Keycloak options
            services.Configure<KeyCloakOptions>(options => configuration.GetSection("KeyCloak").Bind(options));
            // Keycloak authentication
            services.AddKeyCloakAuthentication(configuration.GetSection("KeyCloak"));
            // Keycloak Roles Authorization
            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
            services.AddTransient<IKeycloakUserService, KeycloakUserService>();
        }

        public static void AddSeeders(this IServiceCollection services)
        {
            services.AddTransient<InitialSeeder>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ISprintService, SprintService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<ICommentService, CommentService>();

            //services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<ICourseService, CourseService>();
            //services.AddScoped<ISectionService, SectionService>();
            //services.AddScoped<IChapterService, ChapterService>();
            //services.AddScoped<IQuestionService, QuestionService>();
            //services.AddScoped<IAnswerService, AnswerService>();
            //services.AddScoped<ITagService, TagService>();
            //services.AddScoped<IRoleApplicationService, RoleApplicationService>();
        }

        public static void AddAuthorizationHandler(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
        }
        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("LocalDB"),
                new MySqlServerVersion(new Version(10, 6, 4))
                , options => options.EnableRetryOnFailure()
                )
             );
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Title",
                    Version = "v1",
                    Description = "Description",
                    Contact = new OpenApiContact
                    {
                        Name = "Grecu Bogdan",
                        Email = "bogdan.grecu13@s.unibuc.ro",
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });



                //c.IncludeXmlComments("Yello.Api.XML");

                c.EnableAnnotations();
            });
        }

        public static void AddMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AllowNullCollections = true;
                config.AllowNullDestinationValues = true;
                config.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "allowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
                    });
            });
        }
    }
}
