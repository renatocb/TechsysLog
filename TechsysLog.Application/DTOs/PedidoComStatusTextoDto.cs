using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechsysLog.Application.DTOs
{
    public class PedidoComStatusTextoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataPedido { get; set; }
        public string NumeroPedido { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string StatusPedido { get; set; }
        public EntregaDto Entrega { get; set; } 
    }
}
