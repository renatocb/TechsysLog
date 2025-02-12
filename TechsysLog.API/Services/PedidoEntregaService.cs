using Microsoft.AspNetCore.SignalR;
using TechsysLog.Application.DTOs;
using TechsysLog.Application.Hubs;
using TechsysLog.Application.Interfaces;
using TechsysLog.Domain.Entities;
using TechsysLog.Domain.Enums;
using TechsysLog.Domain.Interfaces;

namespace TechsysLog.API.Services
{
    public class PedidoEntregaService : IPedidoEntregaService
    {
        private readonly IHubContext<PedidoEntregaHub> _hubContext;

        public PedidoEntregaService(IHubContext<PedidoEntregaHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotificarPedidoAsync(string userId, Pedido pedido, string message)
        {
            var pedidoComStatusTexto = new PedidoComStatusTextoDto
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                DataPedido = pedido.DataPedido,
                NumeroPedido = pedido.NumeroPedido,
                Descricao = pedido.Descricao,
                Valor = pedido.Valor,
                Cep = pedido.Cep,
                Rua = pedido.Rua,
                Numero = pedido.Numero,
                Bairro = pedido.Bairro,
                Cidade = pedido.Cidade,
                Estado = pedido.Estado,
                StatusPedido = ConverterStatusParaTexto(pedido.StatusPedido),
                Entrega = pedido.Entrega != null ? new EntregaDto // Mapeia o objeto Entrega
                {
                    Id = pedido.Entrega.Id,
                    PedidoId = pedido.Entrega.PedidoId,
                    UsuarioId = pedido.Entrega.UsuarioId,
                    Status = pedido.Entrega.Status,
                    DataHoraEntrega = pedido.Entrega.DataHoraEntrega
                } : new EntregaDto() // Provide a default value to avoid null reference
            };

            Console.WriteLine($"Enviando notificação: Pedido = {pedido.NumeroPedido}, Mensagem = {message}");

            await _hubContext.Clients.All.SendAsync("ReceivePedidoNotification", new { Pedido = pedidoComStatusTexto, Mensagem = message });
        }

        public async Task NotificarEntregaAsync(string userId, Pedido pedido, string message)
        {
            var pedidoComStatusTexto = new PedidoComStatusTextoDto
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                DataPedido = pedido.DataPedido,
                NumeroPedido = pedido.NumeroPedido,
                Descricao = pedido.Descricao,
                Valor = pedido.Valor,
                Cep = pedido.Cep,
                Rua = pedido.Rua,
                Numero = pedido.Numero,
                Bairro = pedido.Bairro,
                Cidade = pedido.Cidade,
                Estado = pedido.Estado,
                StatusPedido = ConverterStatusParaTexto(pedido.StatusPedido),
                Entrega = pedido.Entrega != null ? new EntregaDto // Mapeia o objeto Entrega
                {
                    Id = pedido.Entrega.Id,
                    PedidoId = pedido.Entrega.PedidoId,
                    UsuarioId = pedido.Entrega.UsuarioId,
                    Status = pedido.Entrega.Status,
                    DataHoraEntrega = pedido.Entrega.DataHoraEntrega
                } : new EntregaDto() // Provide a default value to avoid null reference
            };

            Console.WriteLine($"Enviando notificação: Entrega = {pedido.Id}, Mensagem = {message}");

            await _hubContext.Clients.All.SendAsync("ReceivePedidoNotification", new { Pedido = pedidoComStatusTexto, Mensagem = message });
        }

        private static string ConverterStatusParaTexto(StatusPedido status)
        {
            switch (status)
            {
                case StatusPedido.AguardandoPagamento:
                    return "Aguardando Pagamento";
                case StatusPedido.Pago:
                    return "Pago";
                case StatusPedido.Processando:
                    return "Processando";
                case StatusPedido.Enviado:
                    return "Enviado";
                case StatusPedido.Entregue:
                    return "Entregue";
                case StatusPedido.Cancelado:
                    return "Cancelado";
                default:
                    return "Status Desconhecido";
            }
        }       
    }
}
