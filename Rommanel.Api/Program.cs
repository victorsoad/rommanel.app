using Rommanel.Domain.Repositories;
using Rommanel.Infrastructure.Persistence;
using Rommanel.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rommanel.Application;
using Rommanel.Application.Validators;
using FluentValidation;
using Microsoft.Data.Sqlite;
using Rommanel.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext with SQLite In-Memory
var connection = new SqliteConnection("Data Source=Data/rommanel.db");
connection.Open();
builder.Services.AddDbContext<CustomerDbContext>(options =>
{
    options.UseSqlite(connection);
});

// Dependency Injection for Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// MediatR 12+
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")  // Coloque a URL do seu frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>();

var app = builder.Build();

app.UseCors("AllowFrontend");

// Ensure DB is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    //db.Database.OpenConnection();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.Run();
