using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace HomeDatabase.Helpers
{
    public class WebSocketMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly Helpers.WebSocket_Manager _webSocketManager;

        public WebSocketMiddleware(RequestDelegate next, Helpers.WebSocket_Manager webSocketManager)
        {
            _next = next;
            _webSocketManager = webSocketManager;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var userId = context.Request.Query["userId"]; // Get the userId from the query string or any other way you prefer
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                _webSocketManager.AddSocket(userId, socket);

                // Handle incoming messages
                await _webSocketManager.ReceiveNotificationsAsync(userId, async (message) =>
                {
                    // You can update the UI with the received message here
                    // You may want to use SignalR or another mechanism to update the UI dynamically
                    // For simplicity, we'll print the message to the console for now
                    Console.WriteLine($"Received notification for user {userId}: {message}");
                });

                await _webSocketManager.RemoveSocket(userId);
            }
            else
            {
                await _next(context);
            }
        }


    }
}
