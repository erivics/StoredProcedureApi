using DotNet.RateLimiter;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StoredProcedureApi.Utility;
using StoredProcedureApi.Models;
using StoredProcedureApi.Repository;

var builder = WebApplication.CreateBuilder(args);

//Microsoft Logging configured
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

//Log4Net configured
builder.Logging.AddLog4Net("log4net.config")
    .SetMinimumLevel(LogLevel.Trace);



// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(u => u.UseSqlServer(builder.Configuration.GetConnectionString("DConnection")));
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUploadRepo, UploadRepo>();
builder.Services.AddRateLimitService(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "StoredProcedure API", Version = "v1"});
});
builder.Services.ConfigureSQL(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StoredProcedureAPI v1");
        c.RoutePrefix = string.Empty;
    });
}
//configure the exception middleware
app.UseMiddleware(typeof(GlobalErrorHandlingMiddleware));
app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
