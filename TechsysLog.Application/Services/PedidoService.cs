using AutoMapper;
using TechsysLog.Application.DTOs;
using TechsysLog.Application.Interfaces;
using TechsysLog.Domain.Entities;
using TechsysLog.Domain.Enums;
using TechsysLog.Domain.Interfaces;
using System.Threading.Tasks;

namespace TechsysLog.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IRepositorio<Pedido> _pedidoRepositorio;
        private readonly INotificationRepositorio _notificationRepositorio;
        private readonly IMapper _mapper;
        private readonly IPedidoEntregaService _pedidoEntregaService;
        private readonly IRepositorio<Entrega> _entregaRepositorio;

        public PedidoService(
            IRepositorio<Pedido> pedidoRepositorio,
            IMapper mapper,
            INotificationRepositorio notificationRepositorio,
            IPedidoEntregaService pedidoEntregaService,
            IRepositorio<Entrega> entregaRepositorio
        )
        {
            _pedidoRepositorio = pedidoRepositorio;
            _mapper = mapper;
            _notificationRepositorio = notificationRepositorio;
            _pedidoEntregaService = pedidoEntregaService;
            _entregaRepositorio = entregaRepositorio;
        }

        public async Task<IEnumerable<ResponsePedidoDto>> ObterTodosAsync()
        {
            var pedidos = await _pedidoRepositorio.ObterTodosAsync(p => p.Entrega); // Inclui a entidade Entrega
            return _mapper.Map<IEnumerable<ResponsePedidoDto>>(pedidos);
        }

        public async Task<ResponsePedidoDto> ObterPorIdAsync(int id)
        {
            var pedido = await _pedidoRepositorio.ObterPorIdAsync(id, p => p.Entrega); // Inclui a entidade Entrega
            return _mapper.Map<ResponsePedidoDto>(pedido);
        }

        public async Task<Pedido> AdicionarAsync(PedidoDto pedidoDto)
        {
            // Mapeia o DTO para a entidade Pedido
            var pedido = _mapper.Map<Pedido>(pedidoDto);

            // Adiciona o pedido ao repositório
            await _pedidoRepositorio.AdicionarAsync(pedido);

            // Cria uma entrega associada ao pedido com status Pendente
            var entrega = new Entrega
            {
                PedidoId = pedido.Id, // Associa a entrega ao pedido recém-criado
                DataHoraEntrega = DateTime.MinValue, // Data de entrega ainda não definida
                Status = StatusEntrega.Pendente, // Status inicial da entrega
                UsuarioId = pedido.UsuarioId // Associa ao mesmo usuário do pedido
            };

            // Adiciona a entrega ao repositório de entregas
            await _entregaRepositorio.AdicionarAsync(entrega);

            // Notifica o usuário sobre o pedido e a entrega criada
            var message = $"Pedido {pedido.NumeroPedido} criado com sucesso. Entrega associada com status Pendente.";
            await SendNotificationAndSaveAsync(pedido.UsuarioId, pedido.NumeroPedido, message, TipoNotificacao.Pedido);

            // Retorna o pedido criado com a entrega associada
            return pedido;
        }

        public async Task AtualizarAsync(PedidoDto pedidoDto)
        {
            var pedido = _mapper.Map<Pedido>(pedidoDto);
            await _pedidoRepositorio.AtualizarAsync(pedido);
        }

        public async Task DeletarAsync(int id)
        {
            await _pedidoRepositorio.DeletarAsync(id);
        }

        public async Task AtualizarStatusAsync(int id, StatusPedido statusPedido)
        {
            try
            {
                var pedido = await _pedidoRepositorio.ObterPorIdAsync(id, p => p.Entrega);
                if (pedido != null)
                {
                    pedido.StatusPedido = statusPedido;

                    await _pedidoRepositorio.AtualizarAsync(pedido);

                    var message = $"O status do pedido {pedido.NumeroPedido} foi atualizado para {statusPedido}";

                    await SendNotificationAndSaveAsync(pedido.UsuarioId, pedido.NumeroPedido, message, TipoNotificacao.Pedido);
                }
            }
            catch (Exception ex)
            {
                // Log ou tratamento da exceção
                Console.WriteLine($"Erro ao atualizar pedido: {ex.Message}");
                throw; // Re-lança a exceção se necessário
            }
        }

        private async Task SendNotificationAndSaveAsync(int usuarioId, string numeroPedido, string message, TipoNotificacao tipo)
        {
            var pedido = await _pedidoRepositorio.ObterPorNumeroAsync(numeroPedido); // Recupera o pedido do banco de dados pelo número

            if (pedido != null)
            {
                // Envia a notificação via SignalR
                await _pedidoEntregaService.NotificarPedidoAsync(usuarioId.ToString(), pedido, message);

                // Salva a notificação no banco de dados
                var notification = new Notification
                {
                    UsuarioId = usuarioId,
                    Tipo = tipo,
                    Message = message,
                    Date = DateTime.UtcNow,
                    IsRead = false
                };
                await _notificationRepositorio.AdicionarAsync(notification);
            }
            else
            {
                Console.WriteLine("Pedido não encontrado.");
            }
        }
    }
}