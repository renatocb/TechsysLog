using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechsysLog.Domain.Enums
{    public enum StatusPedido
    {
        AguardandoPagamento, // O pedido foi criado, mas o pagamento ainda não foi confirmado.
        Pago,               // O pagamento foi confirmado, e o pedido está pronto para ser processado.
        Processando,         // O pedido está sendo preparado para envio.
        Enviado,             // O pedido foi enviado ao cliente.
        Entregue,            // O pedido foi entregue ao cliente.
        Cancelado            // O pedido foi cancelado (antes ou após o pagamento).
    }
}
