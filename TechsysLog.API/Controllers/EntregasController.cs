using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TechsysLog.Application.DTOs;
using TechsysLog.Application.Interfaces;
using TechsysLog.Domain.Enums;

namespace TechsysLog.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EntregasController : ControllerBase
    {
        private readonly IEntregaService _entregaService;

        public EntregasController(IEntregaService entregaService)
        {
            _entregaService = entregaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodasEntregas()
        {
            var entregas = await _entregaService.ObterTodasAsync();
            return Ok(entregas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterEntregaPorId(int id)
        {
            var entrega = await _entregaService.ObterPorIdAsync(id);
            if (entrega == null)
            {
                return NotFound();
            }
            return Ok(entrega);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarEntrega(EntregaDto entregaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //await _entregaService.AdicionarAsync(entregaDto);

            await _entregaService.AdicionarAsync(entregaDto);
            return CreatedAtAction(nameof(ObterEntregaPorId), new { id = entregaDto.Id }, entregaDto);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatusEntrega(int id, [FromBody] string statusTexto, DateTime? dataHoraEntrega)
        {
            try
            {
                // Converte o texto para o enum StatusEntrega, ignorando maiúsculas/minúsculas
                if (!Enum.TryParse(statusTexto, ignoreCase: true, out StatusEntrega status))
                {
                    // Lista os valores válidos do enum para retornar uma mensagem de erro mais útil
                    var valoresValidos = Enum.GetNames(typeof(StatusEntrega));
                    return BadRequest($"Status inválido. Valores válidos: {string.Join(", ", valoresValidos)}");
                }

                // Chama o serviço para atualizar o status
                await _entregaService.AtualizarStatusAsync(id, status, dataHoraEntrega);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                // Captura exceções de argumento inválido e retorna BadRequest
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Captura exceções de operação inválida e retorna BadRequest
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Captura outras exceções e retorna um erro genérico
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarEntrega(int id, EntregaDto entregaDto)
        {
            if (id != entregaDto.Id)
            {
                return BadRequest();
            }

            await _entregaService.AtualizarAsync(entregaDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarEntrega(int id)
        {
            await _entregaService.DeletarAsync(id);
            return NoContent();
        }

    }
}