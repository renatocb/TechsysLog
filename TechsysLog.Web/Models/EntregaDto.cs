using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechsysLog.Web.Models
{
    public class EntregaDto
    {
        public int Id { get; set; }        
        public int PedidoId { get; set; }        
        public int UsuarioId { get; set; }        
        public string Status { get; set; }        
        public DateTime? DataHoraEntrega { get; set; }
    }
}
