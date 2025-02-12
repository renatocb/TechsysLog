using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechsysLog.Application.DTOs;
using TechsysLog.Application.Interfaces;
using TechsysLog.Domain.Entities;

namespace TechsysLog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosUsuarios()
        {
            var usuarios = await _usuarioService.ObterTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuarioPorId(int id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarUsuario(RegistroUsuarioDto registroDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _usuarioService.AdicionarAsync(registroDto);

            var usuario = await _usuarioService.ObterPorEmailAsync(registroDto.Email);

            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
            return CreatedAtAction(nameof(ObterUsuarioPorId), new { id = usuarioDto.Id }, usuarioDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, UsuarioDto usuarioDto)
        {
            if (id != usuarioDto.Id)
            {
                return BadRequest();
            }

            await _usuarioService.AtualizarAsync(usuarioDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            await _usuarioService.DeletarAsync(id);
            return NoContent();
        }
    }
}
