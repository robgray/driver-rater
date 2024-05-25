namespace DriverRater.Api.Plumbing.Startup.Swagger;

using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

public static class SwaggerStartup
{
    internal static string[] Versions => new[]
    {
        "v1",
    };
    
    private static SwaggerGenOptions DecorateSwaggerWithCodeComments(this SwaggerGenOptions options)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            string str = Path.Combine(AppContext.BaseDirectory, assembly.GetName().Name + ".xml");
            if (File.Exists(str))
            {
                options.IncludeXmlComments(str);
            }
        }

        return options;
    }
    
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.DecorateSwaggerWithCodeComments();
            
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header, 
                Description = "Please insert 'Bearer ' and the then JWT into the field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey 
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { 
                    new OpenApiSecurityScheme 
                    { 
                        Reference = new OpenApiReference 
                        { 
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer" 
                        } 
                    },
                    new string[] { } 
                } 
            });
        });

        return services;
    }
    
    public static IServiceCollection AddAuth0Swagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(
            options =>
            {
                options.DecorateSwaggerWithCodeComments();

                options.CustomSchemaIds(type => type.FullName.Replace("+", "_"));
                options.CustomOperationIds(apiDesc => apiDesc.ActionDescriptor.Id);

                options.AddSecurityDefinition(
                    SecuritySchemeType.OAuth2.ToString(),
                    new OpenApiSecurityScheme()
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows()
                        {
                            ClientCredentials = new OpenApiOAuthFlow()
                            {
                                TokenUrl = new Uri($"{configuration["Authentication:Authority"]}/oauth/token"),
                                Extensions = new Dictionary<string, IOpenApiExtension>()
                                {
                                    {
                                        "audience", new OpenApiString(configuration["Authentication:Audience"])
                                    }
                                }
                            }
                        }
                    });

                OpenApiSecurityRequirement securityRequirement = new();
                OpenApiSecurityScheme key = new()
                {
                    Type = SecuritySchemeType.OAuth2,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = SecuritySchemeType.OAuth2.ToString(),
                    },
                };
                securityRequirement.Add(key, Array.Empty<string>());
                options.AddSecurityRequirement(securityRequirement);

                foreach (var version in Versions)
                    options.SwaggerDoc(version, CreateInfoForApiVersion(version));
            });

        return services;
    }
    
    private static OpenApiInfo CreateInfoForApiVersion(string version)
    {
        var info = new OpenApiInfo
        {
            Title = "Driver Rater API",
            Version = version,
            Contact = new OpenApiContact
            {
                Name = "Rob Gray",
                Email = "rob.gray@outlook.com"
            },
            Description = string.Empty,
        };
        return info;
    }

    public static IApplicationBuilder UseCustomSwagger(
        this IApplicationBuilder app,
        IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            foreach (string availableApiVersion in Versions)
            {
                c.SwaggerEndpoint($"swagger/{availableApiVersion}/swagger.json", availableApiVersion.ToLowerInvariant());
            }
            c.RoutePrefix = string.Empty;
            c.DocExpansion(DocExpansion.List);
            c.DefaultModelExpandDepth(3);
            c.DefaultModelsExpandDepth(0);
            
            c.UseRequestInterceptor(
                "(req) => { if (req.url.endsWith('oauth/token') && req.body) req.body += '&audience=" +
                configuration["Authentication:Audience"] + "'; return req; }");
        });

        return app;
    }
}