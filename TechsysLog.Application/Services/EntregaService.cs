using AutoMapper;
using global::TechsysLog.Application.DTOs;
using global::TechsysLog.Application.Interfaces;
using global::TechsysLog.Domain.Entities;
using global::TechsysLog.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using TechsysLog.Domain.Enums;
using TechsysLog.Infrastructure.Repositories;


namespace TechsysLog.Application.Services
{
    public class EntregaService : IEntregaService
    {
        private readonly IRepositorio<Entrega> _entregaRepositorio;
        private readonly IRepositorio<Pedido> _pedidoRepositorio;        
        private readonly INotificationRepositorio _notificationRepositorio;
        private readonly IPedidoEntregaService _pedidoEntregaService;
        private readonly IMapper _mapper;

        public EntregaService(IRepositorio<Entrega> entregaRepositorio, IMapper mapper, INotificationRepositorio notificationRepositorio, IRepositorio<Pedido> pedidoRepositorio, IPedidoEntregaService pedidoEntregaService)
        {
            _entregaRepositorio = entregaRepositorio;
            _pedidoRepositorio = pedidoRepositorio;
            _notificationRepositorio = notificationRepositorio;
            _pedidoEntregaService = pedidoEntregaService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EntregaDto>> ObterTodasAsync()
        {
            var entregas = await _entregaRepositorio.ObterTodosAsync();
            return _mapper.Map<IEnumerable<EntregaDto>>(entregas);
        }

        public async Task<EntregaDto> ObterPorIdAsync(int id)
        {
            var entrega = await _entregaRepositorio.ObterPorIdAsync(id);
            return _mapper.Map<EntregaDto>(entrega);
        }

        public async Task AdicionarAsync(EntregaDto entregaDto)
        {
            var pedido = await _pedidoRepositorio.ObterPorIdAsync(entregaDto.PedidoId);

            if (pedido == null)
            {
                throw new ArgumentException("Pedido não encontrado.");
            }

            if (entregaDto.DataHoraEntrega < pedido.DataPedido)
            {
                throw new ValidationException("A data da entrega não pode ser anterior à data do pedido.");
            }

            // Verifica se o pedido está pago
            if (pedido.StatusPedido != StatusPedido.Pago)
            {
                throw new InvalidOperationException("A entrega só pode ser cadastrada para pedidos pagos.");
            }


            var entrega = _mapper.Map<Entrega>(entregaDto);
            await _entregaRepositorio.AdicionarAsync(entrega);
        }

        public async Task AtualizarAsync(EntregaDto entregaDto)
        {
            var entrega = _mapper.Map<Entrega>(entregaDto);
            await _entregaRepositorio.AtualizarAsync(entrega);
        }

        public async Task DeletarAsync(int id)
        {
            await _entregaRepositorio.DeletarAsync(id);
        }

        public async Task AtualizarStatusAsync(int id, StatusEntrega status, DateTime? dataHoraEntrega = null)
        {
            var entrega = await _entregaRepositorio.ObterPorIdAsync(id);

            if (entrega == null)
            {
                throw new ArgumentException("Entrega não encontrada.");
            }

            // Verifica se o status atual da entrega é "Entregue"
            if (entrega.Status == StatusEntrega.Entregue)
            {
                throw new InvalidOperationException("Não é possível atualizar uma entrega com o status 'Entregue'.");
            }

            var pedido = await _pedidoRepositorio.ObterPorIdAsync(entrega.PedidoId);
            if (pedido == null)
            {
                throw new ArgumentException("Pedido não encontrado.");
            }

            // Verifica se o pedido já foi entregue
            if (pedido.StatusPedido == StatusPedido.Entregue)
            {
                throw new InvalidOperationException("Não é possível atualizar o status da entrega após o pedido ter sido entregue.");
            }

            // Validação específica para o status "Entregue"
            if (status == StatusEntrega.Entregue)
            {
                if (!dataHoraEntrega.HasValue)
                {
                    throw new ArgumentException("A data e hora da entrega são obrigatórias quando o status é 'Entregue'.");
                }

                if (dataHoraEntrega.Value < pedido.DataPedido)
                {
                    throw new ArgumentException("A data e hora da entrega não podem ser menores do que a data do pedido.");
                }

                // Atribui a data de entrega
                entrega.DataHoraEntrega = dataHoraEntrega.Value;
            }

            // Atualiza o status da entrega
            entrega.Status = status;
            await _entregaRepositorio.AtualizarAsync(entrega);

            // Envia notificação
            var message = $"O status da entrega do pedido {entrega.PedidoId} foi atualizado para {status}";
            await SendNotificationAndSaveAsync(entrega.UsuarioId, entrega, message, TipoNotificacao.Entrega);
        }
        private async Task SendNotificationAndSaveAsync(int usuarioId, Entrega entrega, string message, TipoNotificacao tipo)
        {

            // Busca o pedido relacionado à entrega
            var pedido = await _pedidoRepositorio.ObterPorIdAsync(entrega.PedidoId, p => p.Entrega);

            if (pedido == null)
            {
                Console.WriteLine("Pedido não encontrado para a entrega.");
                return;
            }

            await _pedidoEntregaService.NotificarEntregaAsync(usuarioId.ToString(), pedido, message);

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
    }
}

