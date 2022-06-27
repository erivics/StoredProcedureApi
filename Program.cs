using DotNet.RateLimiter;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StoredProcedureApi.Models;
using StoredProcedureApi.Repository;

var builder = WebApplication.CreateBuilder(args);

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
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StoredProcedureAPI v1"));
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
