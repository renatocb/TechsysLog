using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Domain.Enums;

namespace TechsysLog.Domain.Entities;

public class Entrega
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }    
    public DateTime DataHoraEntrega { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public StatusEntrega Status { get; set; } = StatusEntrega.Pendente;
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }
}

