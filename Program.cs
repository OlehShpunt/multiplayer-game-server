var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Application.IGameStateManager, Application.GameStateManager>();
builder.Services.AddSingleton(
    typeof(Application.IStateManager<>),
    typeof(Application.StateManager<>)
);

var app = builder.Build();

app.Run();
