using Microsoft.OpenApi;

namespace M223PunchclockDotnet;

public static class Startup
{
    public static void AddCustomSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "JwtAuthApp API", Version = "v1" });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = """
                              JWT Authorization header using the Bearer scheme. 
                              Enter 'Bearer' [space] and then your token in the text input below.
                              Example: 'Bearer 12345abcdef'
                              """,
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Scheme = "Bearer",
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("http://localhost:5121/login/oauth/authorize"),
                        TokenUrl = new Uri("http://localhost:5121/login/oauth/access_token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "api", "Access to API" }
                        }
                    }
                }
            });

            options.AddSecurityRequirement(doc =>
            {
                OpenApiSecurityRequirement requirement = new()
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", doc),
                        ["api"]
                    }
                };
                return requirement;
            });
        });
    }
}