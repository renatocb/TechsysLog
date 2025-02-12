using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Application.DTOs;
using TechsysLog.Domain.Entities;
using TechsysLog.Domain.Enums;

namespace TechsysLog.Application.Interfaces;

public interface IPedidoService
{
    Task<IEnumerable<ResponsePedidoDto>> ObterTodosAsync();
    Task<ResponsePedidoDto> ObterPorIdAsync(int id);
    Task<Pedido> AdicionarAsync(PedidoDto pedidoDto);
    Task AtualizarAsync(PedidoDto pedidoDto);
    Task DeletarAsync(int id);
    Task AtualizarStatusAsync(int id, StatusPedido statusPedido);
}
