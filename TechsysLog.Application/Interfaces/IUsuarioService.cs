using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Application.DTOs;
using TechsysLog.Domain.Entities;

namespace TechsysLog.Application.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> ObterTodosAsync();
    Task<Usuario> ObterPorEmailAsync(string email);
    Task<UsuarioDto> ObterPorIdAsync(int id);
    Task AdicionarAsync(RegistroUsuarioDto registroDto);
    Task AtualizarAsync(UsuarioDto usuarioDto);
    Task DeletarAsync(int id);
    bool VerificarSenha(string senha, byte[] senhaHash, byte[] senhaSalt);
    bool VerificarSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt);
}
