using BookApi.Command.Persistence;
using BooksCommand.Broker;
using BooksCommand.Database;
using BooksCommand.Domain;
using BooksCommand.Outbox;
using BooksCommand.Persistence;
using Microsoft.EntityFrameworkCore;

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
        // auto database create-drop
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}

app.Run();
