using Microsoft.AspNetCore.Mvc;
using PathFindAPI;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<PathFindLogic>();
builder.Services.AddSingleton<Cell>();
builder.Services.AddSingleton<APIinput>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost("/FindPathBFS", ([FromBody]APIinput input, PathFindLogic pathfindlogic) =>
{
    var path =  pathfindlogic.FindPathBFS(input);
    return path;
});
app.MapPost("/FindPath_AStar", ([FromBody] APIinput input, PathFindLogic pathfindlogic) =>
{
    var path = pathfindlogic.FindPath_AStar(input);
    return path;
});
app.MapGet("/ReceiveBoard", (int gridX, int gridY, PathFindLogic pathfindlogic) =>
{
    pathfindlogic.setBoard(gridX,gridY);
});

app.MapGet("/CreateMaze", (int gridX, int gridY, PathFindLogic pathfindlogic) =>

{
    return pathfindlogic.createMaze(gridX, gridY);
});

app.Run();