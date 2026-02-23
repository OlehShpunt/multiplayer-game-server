using API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Application.IGameStateManager, Application.GameStateManager>();
builder.Services.AddSingleton(
    typeof(Application.IStateManager<>),
    typeof(Application.StateManager<>)
);

var app = builder.Build();

app.UseWebSockets();
WebSocketService webSocketService = new WebSocketService();
app.Map("/ws", webSocketService.HandleNewConnectionAsync);
app.Lifetime.ApplicationStopping.Register(() =>
{
    webSocketService.DisconnectAllClients();
});

app.Run();
