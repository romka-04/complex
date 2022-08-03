using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Romka04.Complex.Core;
using MiniValidation;
using Romka04.Complex.Models;
using Romka04.Complex.WebApi;
using Romka04.Complex.WebApi.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>((provider, options) =>
{
    var connStr = provider.GetService<IConfiguration>().GetConnectionString("pgDatabase");
    options.UseNpgsql(connStr);
});
builder.Services.AddOptions<RedisOptions>(RedisOptions.Name);
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/values/all", async (HttpContext context) =>
{
    await using var db = context.RequestServices.GetService<DatabaseContext>();
    var dto = await db.Values.ToArrayAsync();
    var res = dto.Select(x => new FabValue(x.Number)).ToArray();
    return Results.Ok(res);
});

app.MapGet("/values/current", async (HttpContext context) =>
{
    var redisOptions = context.RequestServices.GetService<IOptions<RedisOptions>>()?.Value;
    await Task.Delay(1);
    // request redis for values that were used during current session
    var result = new FabPair[]
    {
        new(0, 0),
        new(1, 1),
        new(2, 1),
        new(3, 2),
        new(4, 3),
        new(5, 5),
        new(6, 8),
        new(19, 4181),
    };

    return result;
});

app.MapPost("/values", async (FabRequest request, HttpContext context) =>
{
    if (!MiniValidator.TryValidate(request, out var errors))
        return Results.BadRequest(errors);

    // calc new value and add it to database and redis
    // ...
    await Task.Delay(1);
    var index = request.Index.Value;
    var value = FabCalculator.Fab(index);
    var response = new FabResponse(index, value);

    return Results.Ok(response);
});

app.Run();
