using Microsoft.EntityFrameworkCore;
using WebApiCaching.Data;
using WebApiCaching.Repository;
using WebApiCaching.Service;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
        options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString"));
});
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IJogadorRepository, JogadorRepository>();
builder.Services.AddScoped<ITimeFutebolRepository, TimeFutebolRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
