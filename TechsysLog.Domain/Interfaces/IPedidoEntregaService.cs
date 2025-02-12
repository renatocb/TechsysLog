using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Domain.Entities;

namespace TechsysLog.Domain.Interfaces
{
    public interface IPedidoEntregaService
    {
        Task NotificarPedidoAsync(string userId, Pedido pedido, string message);
        Task NotificarEntregaAsync(string userId, Pedido entrega, string message);
    }
}
