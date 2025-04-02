using BlazorPruebas.Components;
using Microsoft.AspNetCore.SignalR; //  Importar SignalR

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSignalR(); //  Agregar SignalR

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

/* app.UseHttpsRedirection(); */


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<ClickHub>("/clickhub"); //  Registrar el Hub de SignalR
app.MapHub<ChatHub>("/chathub"); //  Registrar el hub del chat

app.Run();



// Definir el Hub de SignalR
public class ClickHub : Hub
{
    private static int contador = 0;

    public async Task IncrementarContador()
    {
        contador++;
        await Clients.All.SendAsync("ActualizarContador", contador); // 🔥 Enviar a todos
    }

    public async Task ObtenerContador()
    {
        await Clients.Caller.SendAsync("ActualizarContador", contador); // 🔥 Enviar al usuario nuevo
    }
}



// 🔹 Definir el Hub de SignalR para el chat
public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message); // 🔥 Enviar mensaje a todos
    }
}