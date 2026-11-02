using M223PunchclockDotnet.Database;
using M223PunchclockDotnet.Models;
using M223PunchclockDotnet.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IService<Category>, CategoryService>();
builder.Services.AddScoped<IService<Entry>, EntryService>();
builder.Services.AddScoped<IService<EntryDetail>, EntryDetailService>();
builder.Services.AddScoped<IService<Tag>, TagService>();

builder.Services.AddScoped<ExampleDataService>();

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql("Host=db;Database=postgres;Username=postgres;Password=postgres"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
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

app.UseAuthorization();

app.MapControllers();

app.Run();