using Microsoft.AspNetCore.Builder;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace HomeDatabase.Helpers
{
    public class WebSocket_Manager
    {

        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public void AddSocket(string userId, WebSocket socket)
        {
            _sockets.TryAdd(userId, socket);
        }

        public async Task RemoveSocket(string userId)
        {
            if (_sockets.TryRemove(userId, out var socket))
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the server", System.Threading.CancellationToken.None);
            }
        }

        public async Task SendNotificationAsync(string userId, string message)
        {
            if (_sockets.TryGetValue(userId, out var socket))
            {
                var buffer = new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(message));
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
            }
        }

        public async Task ReceiveNotificationsAsync(string userId, Func<string, Task> handleNotification)
        {
            if (_sockets.TryGetValue(userId, out var socket))
            {
                var buffer = new byte[1024 * 4];
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                        await handleNotification(message);
                    }
                }
            }
        }

    }
}
