using BooksCommand.Domain;
using BooksCommand.Infrastructure.Broker;
using BooksCommand.Infrastructure.Broker.Outbox;
using BooksCommand.Persistence.Context;
using BooksCommand.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IBookFactory, BookFactory>();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddScoped<IBookRepository, BookRepositoryImpl>();
builder.Services.AddSingleton<KafkaProducer>();
builder.Services.AddHostedService<OutbooxPublisher>();

builder.Services.AddDbContext<BooksDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("books-write")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServiceDiscovery(options => options.UseConsul());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BooksDbContext>();
    if (app.Environment.IsDevelopment())
    {
        dbContext.Database.Migrate();
        // auto database create-drop
        //dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();
    }
}

app.Run();
