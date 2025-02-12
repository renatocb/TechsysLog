using AutoMapper;
using System.Security.Cryptography;
using TechsysLog.Application.DTOs;
using TechsysLog.Application.Interfaces;
using TechsysLog.Domain.Entities;
using TechsysLog.Domain.Interfaces;

namespace TechsysLog.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IRepositorio<Usuario> _usuarioRepositorio;
        private readonly IMapper _mapper;        

        public UsuarioService(IRepositorio<Usuario> usuarioRepositorio, IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UsuarioDto>> ObterTodosAsync()
        {
            var usuarios = await _usuarioRepositorio.ObterTodosAsync();
            return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
        }
        public async Task<UsuarioDto> ObterPorIdAsync(int id)
        {
            var usuario = await _usuarioRepositorio.ObterPorIdAsync(id);
            return _mapper.Map<UsuarioDto>(usuario);
        }
        public async Task<Usuario> ObterPorEmailAsync(string email)
        {
            return await _usuarioRepositorio.ObterPorEmailAsync(email);            
        }
        public async Task AdicionarAsync(RegistroUsuarioDto registroDto)
        {
            var usuario = _mapper.Map<Usuario>(registroDto);

            CriarSenhaHash(registroDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);
            usuario.SenhaHash = senhaHash;
            usuario.SenhaSalt = senhaSalt;

            await _usuarioRepositorio.AdicionarAsync(usuario);
        }
        public async Task AtualizarAsync(UsuarioDto usuarioDto)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            await _usuarioRepositorio.AtualizarAsync(usuario);
        }
        public async Task DeletarAsync(int id)
        {
            await _usuarioRepositorio.DeletarAsync(id);
        }
        private void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                senhaSalt = hmac.Key;
                senhaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
            }
        }
        public bool VerificarSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt)
        {
            using (var hmac = new HMACSHA512(senhaSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
                return computedHash.SequenceEqual(senhaHash);
            }
        }
        public bool VerificarSenha(string senha, byte[] senhaHash, byte[] senhaSalt)
        {
            using (var hmac = new HMACSHA512(senhaSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
                return computedHash.SequenceEqual(senhaHash);
            }
        }
    }
}