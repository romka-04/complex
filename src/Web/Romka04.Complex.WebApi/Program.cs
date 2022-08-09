using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Romka04.Complex.Core;
using MiniValidation;
using Romka04.Complex.Models;
using Romka04.Complex.WebApi;
using Romka04.Complex.WebApi.Database;
using Romka04.Complex.WebApi.Database.Entities;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>((provider, options) =>
{
    var connStr = provider.GetService<IConfiguration>().GetConnectionString("pgDatabase");
    options.UseNpgsql(connStr);
});
builder.Services.AddOptions<RedisOptions>().Bind(builder.Configuration.GetSection(RedisOptions.Name));
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");


app.MapGet("/values/all", async (HttpContext context) =>
{
    await using var db = context.RequestServices.GetService<DatabaseContext>();
    var dto = await db.Values.ToArrayAsync();
    var res = dto.Select(x => new FabValue(x.Number)).ToArray();
    return Results.Ok(res);
});

app.MapGet("/values/current", async (HttpContext context) =>
{
    var redisOptions = GetRedisOptions<RedisOptions>(context);

    using ConnectionMultiplexer redis = ConnectionMultiplexer
        .Connect(redisOptions.GetConfiguration());
    var db = redis.GetDatabase();
    
    var res = new List<FabPair>();
    await foreach (var dto in db.HashScanAsync("values"))
    {
        res.Add(FabPair.Parse(dto.Name, dto.Value));
    }

    return Results.Ok(res.ToArray());
});

app.MapPost("/values", async (FabRequest request, HttpContext context) =>
{
    if (!MiniValidator.TryValidate(request, out var errors))
        return Results.BadRequest(errors);

    // calc new value
    var index = request.Index.Value;
    var value = FabCalculator.Fab(index);
    var response = new FabResponse(index, value);

    // add it to database and redis
    var redisOption = GetRedisOptions<RedisOptions>(context);
    var t1 = SetRedis(index, value, redisOption);

    var t2 = SetDatabase(index, value, context);
    
    await TaskExt.WhenAll(t1, t2);

    return Results.Ok(response);
});

app.Run();

async Task SetRedis(int index, int value, RedisOptions redisOptions)
{
    using ConnectionMultiplexer redis = ConnectionMultiplexer
        .Connect(redisOptions.GetConfiguration());
    var db = redis.GetDatabase();

    await db.HashSetAsync("values", new HashEntry[] { new(index, value) });
}

async Task SetDatabase(int index, int value, HttpContext context, CancellationToken cancellationToken = default)
{
    await using var db = context.RequestServices.GetService<DatabaseContext>();
    var newEntity = new ValuesEntity { Number = index };
    var exists = await db.Values.AnyAsync(x => x.Number == newEntity.Number);
    if (!exists)
    {
        await db.Values.AddAsync(newEntity);
        await db.SaveChangesAsync();
    }
}

TOptions GetRedisOptions<TOptions>(HttpContext context)
    where TOptions : class
{
    if (null == context) throw new ArgumentNullException(nameof(context));

    var options = context.RequestServices.GetService<IOptions<TOptions>>();
    if (null == options) throw new Exception("Please register options during service registration");

    return options.Value;
}



