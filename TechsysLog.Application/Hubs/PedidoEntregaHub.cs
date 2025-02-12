using Microsoft.AspNetCore.SignalR;
using TechsysLog.Domain.Entities;

namespace TechsysLog.Application.Hubs
{
    public class PedidoEntregaHub : Hub
    {
        public async Task SendPedidoNotification(string user, Pedido pedido, string message)
        {
            Console.WriteLine($"Enviando notificação para {user}: {message}");
            
            await Clients.User(user).SendAsync("ReceivePedidoNotification", new { Pedido = pedido, Mensagem = message });
        }

        public async Task SendEntregaNotification(string user, Pedido pedido, string message)
        {
            Console.WriteLine($"Enviando notificação para {user}: {message}");
            
            await Clients.User(user).SendAsync("ReceiveEntregaNotification", new { Pedido = pedido, Mensagem = message });
        }
    }
}
