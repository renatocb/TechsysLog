using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Domain.Enums;

namespace TechsysLog.Application.DTOs;

public class PedidoDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "O usuário/cliente do Pedido é obrigatória.")]
    public required int UsuarioId { get; set; }    
    [Required(ErrorMessage = "O número do pedido é obrigatório.")]
    [StringLength(20, ErrorMessage = "O número do pedido deve ter no máximo 20 caracteres.")]
    public required string NumeroPedido { get; set; }
    [Required(ErrorMessage = "A descrição do pedido é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição do pedido deve ter no máximo 500 caracteres.")]
    public required string Descricao { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor do pedido deve ser maior que zero.")]
    public decimal Valor { get; set; }
    [Required(ErrorMessage = "O CEP é obrigatório.")]    
    public required string Cep { get; set; }
    [Required(ErrorMessage = "A rua é obrigatória.")]
    [StringLength(200, ErrorMessage = "A rua deve ter no máximo 200 caracteres.")]
    public string Rua { get; set; }
    [Required(ErrorMessage = "O número é obrigatório.")]
    [StringLength(10, ErrorMessage = "O número deve ter no máximo 10 caracteres.")]
    public required string Numero { get; set; }
    [Required(ErrorMessage = "O bairro é obrigatório.")]
    [StringLength(100, ErrorMessage = "O bairro deve ter no máximo 100 caracteres.")]
    public required string Bairro { get; set; }
    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [StringLength(100, ErrorMessage = "A cidade deve ter no máximo 100 caracteres.")]
    public required string Cidade { get; set; }
    [Required(ErrorMessage = "O estado é obrigatório.")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "O estado deve ter exatamente 2 caracteres.")]
    public required string Estado { get; set; }    
}

