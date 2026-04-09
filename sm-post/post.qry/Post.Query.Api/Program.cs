
using Confluent.Kafka;
using CQRS.core.Consumers;
using CQRS.core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrasturcture.Consumers;
using Post.Query.Infrasturcture.DataAccess;
using Post.Query.Infrasturcture.Dispatchers;
using Post.Query.Infrasturcture.Handlers;
using Post.Query.Infrasturcture.Repositories;

var builder = WebApplication.CreateBuilder(args);

// using lazy loading proxies and sql server for database context
Action<DbContextOptionsBuilder> configureDbContext = (o => o.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDbContext<DataBaseContext>(configureDbContext);
builder.Services.AddSingleton<DataBaseContextFactory>(new DataBaseContextFactory(configureDbContext));
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumers>();
builder.Services.AddHostedService<ConsumerHostedService>();

// create Database from code

var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DataBaseContext>();
dataContext.Database.EnsureCreated();


// DI

builder.Services.AddScoped<IPostRepository, PostRepositoy>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();
builder.Services.AddScoped<IEventHandler, Post.Query.Infrasturcture.Handlers.EventHandler>();

var queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IQueryHandler>();

var dispatcher = new QueryDispatcher();

dispatcher.RegisterHandler<FindAllPostQueries>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindPostByAuthorQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindPostByIdQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindPostWithCommentsQuery>(queryHandler.HandleAsync);
dispatcher.RegisterHandler<FindPostWithLikesQuery>(queryHandler.HandleAsync);

builder.Services.AddSingleton<IQueryDispatcher<PostEntity>>(_=dispatcher);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
// Enable routing
app.UseRouting();

// Map controller endpoints
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
