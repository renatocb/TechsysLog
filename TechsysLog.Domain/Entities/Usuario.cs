using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechsysLog.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required byte[] SenhaHash { get; set; }
    public required byte[] SenhaSalt { get; set; } 
    public ICollection<Pedido> Pedidos { get; set; } = [];
    public ICollection<Entrega> Entregas { get; set; } = [];
}
