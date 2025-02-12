using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Application.DTOs;
using TechsysLog.Domain.Enums;

namespace TechsysLog.Application.Interfaces;

public interface IEntregaService
{
    Task<IEnumerable<EntregaDto>> ObterTodasAsync();
    Task<EntregaDto> ObterPorIdAsync(int id);
    Task AdicionarAsync(EntregaDto entregaDto);
    Task AtualizarAsync(EntregaDto entregaDto);
    Task AtualizarStatusAsync(int id, StatusEntrega status, DateTime? dataHoraEntrega = null);
    Task DeletarAsync(int id);
}
