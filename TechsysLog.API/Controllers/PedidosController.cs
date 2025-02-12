using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechsysLog.Application.DTOs;
using TechsysLog.Application.Interfaces;
using TechsysLog.Domain.Entities;
using TechsysLog.Domain.Enums;

namespace TechsysLog.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]    
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;
        private readonly IMapper _mapper;

        public PedidosController(IPedidoService pedidoService, IMapper mapper)
        {
            _pedidoService = pedidoService;
            _mapper = mapper;   
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosPedidos()
        {
            var pedidos = await _pedidoService.ObterTodosAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPedidoPorId(int id)
        {
            var pedido = await _pedidoService.ObterPorIdAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarPedido(PedidoDto pedidoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Adiciona o pedido e retorna o objeto criado
            var pedidoCriado = await _pedidoService.AdicionarAsync(pedidoDto);

            // Mapeia o pedido criado para o DTO de resposta
            var responseDto = _mapper.Map<ResponsePedidoDto>(pedidoCriado);                        

            // Retorna o pedido criado com a entrega associada
            return CreatedAtAction(nameof(ObterPedidoPorId), new { id = responseDto.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPedido(int id, PedidoDto pedidoDto)
        {
            if (id != pedidoDto.Id)
            {
                return BadRequest();
            }

            await _pedidoService.AtualizarAsync(pedidoDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPedido(int id)
        {
            await _pedidoService.DeletarAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatusPedido(int id, [FromBody] string statusTexto)
        {
            // Converte o texto para o enum StatusPedido, ignorando maiúsculas/minúsculas
            if (!Enum.TryParse(statusTexto, ignoreCase: true, out StatusPedido status))
            {
                // Lista os valores válidos do enum para retornar uma mensagem de erro mais útil
                var valoresValidos = Enum.GetNames(typeof(StatusPedido));
                return BadRequest($"Status inválido. Valores válidos: {string.Join(", ", valoresValidos)}");
            }

            await _pedidoService.AtualizarStatusAsync(id, status);

            return NoContent();
        }
    }
}