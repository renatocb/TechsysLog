using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Domain.Enums;

namespace TechsysLog.Domain.Entities;
public class Pedido
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public DateTime DataPedido { get; set; } = DateTime.UtcNow;
    public required string NumeroPedido { get; set; }
    public required string Descricao { get; set; }
    public decimal Valor { get; set; }
    public required string Cep { get; set; }
    public required string Rua { get; set; }
    public required string Numero { get; set; }
    public required string Bairro { get; set; }
    public required string Cidade { get; set; }
    public required string Estado { get; set; }
    public Usuario? Usuario { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public StatusPedido StatusPedido { get; set; } = StatusPedido.AguardandoPagamento;

    public Entrega? Entrega { get; set; }
}
