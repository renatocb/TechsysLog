using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TechsysLog.Domain.Enums;

namespace TechsysLog.Application.DTOs;

public class EntregaDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O ID do pedido é obrigatório.")]
    public int PedidoId { get; set; }

    [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
    public int UsuarioId { get; set; }    
    [Required(ErrorMessage = "O status da entrega é obrigatório.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatusEntrega Status { get; set; }

    [Required(ErrorMessage = "A data e hora da entrega é obrigatórias.")]
    public DateTime? DataHoraEntrega { get; set; }   
}
