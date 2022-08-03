using Romka04.Complex.Core;
using MiniValidation;
using Romka04.Complex.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.UseHttpsRedirection();

app.MapGet("/values/all", async () =>
{
    await Task.Delay(1);
    // request database for values
    var res = new[] { 1, 2, 4, 5, 6 };
    return Results.Ok(res);
});

app.MapGet("/values/current", () =>
{
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

    return Task.FromResult(result);
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
