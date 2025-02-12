using Microsoft.AspNetCore.Mvc;
using TechsysLog.Application.DTOs;
using TechsysLog.Application.Interfaces;
using TechsysLog.Domain.Entities;
using TechsysLog.Domain.Interfaces;

namespace TechsysLog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;

        public AuthController(IUsuarioService usuarioService, ITokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _usuarioService.ObterPorEmailAsync(loginDto.Email);

            if (usuario == null)
            {
                return Unauthorized("Usuário não encontrado.");
            }

            if (!_usuarioService.VerificarSenha(loginDto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
            {
                return Unauthorized("Senha incorreta.");
            }

            var token = _tokenService.GerarToken(usuario);

            var usuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };

            return Ok(new
            {
                Token = token,
                Usuario = usuarioDto
            });
        }
    }
}