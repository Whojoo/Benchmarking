using Benchy.DapperVsEfCore.Database;
using Benchy.DapperVsEfCore.Database.Repositories;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();

ValidatorOptions.Global.LanguageManager.Enabled = false;

builder.Services.AddDbContext<VehicleDbContext>(options =>
{
    options.UseSqlServer(DataSchemaConstants.ConnectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped<EfCoreDIRepository>();
builder.Services.AddScoped<DapperRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapFastEndpoints();

app.Run();
