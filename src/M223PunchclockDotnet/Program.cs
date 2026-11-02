using System.Security.Cryptography;
using M223PunchclockDotnet;
using M223PunchclockDotnet.Database;
using M223PunchclockDotnet.Models;
using M223PunchclockDotnet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

RSA publicKey = RSA.Create();
publicKey.ImportFromPem(File.ReadAllText("Keys/public.key"));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(publicKey)
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddScoped<IService<Category>, CategoryService>();
builder.Services.AddScoped<IService<Entry>, EntryService>();
builder.Services.AddScoped<IService<EntryDetail>, EntryDetailService>();
builder.Services.AddScoped<IService<Tag>, TagService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ExampleDataService>();

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql("Host=db;Database=postgres;Username=postgres;Password=postgres"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCustomSwaggerConfig();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0);
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";

        options.OAuthClientId("swagger-ui");
        options.OAuthClientSecret(string.Empty);
        options.OAuthScopes("api");
    });
}

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    DatabaseContext context = services.GetRequiredService<DatabaseContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    // DbInitializer.Initialize(context);

    if (app.Environment.IsDevelopment() && app.Configuration.GetValue<bool>("Punchclock:ExampleData:Enabled"))
    {
        ExampleDataService exampleDataService = services.GetRequiredService<ExampleDataService>();
        await exampleDataService.CreateExampleDataAsync();
    }
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();