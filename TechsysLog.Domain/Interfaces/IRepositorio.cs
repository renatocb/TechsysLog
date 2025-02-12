using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TechsysLog.Domain.Interfaces;

public interface IRepositorio<T> where T : class
{
    Task<IEnumerable<T>> ObterTodosAsync();
    Task<IEnumerable<T>> ObterTodosAsync(params Expression<Func<T, object>>[] includes);
    Task<T?> ObterPorIdAsync(int id);
    Task<T?> ObterPorIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<T?> ObterPorNumeroAsync(string numeroPedido);
    Task<T?> ObterPorEmailAsync(string email);
    Task AdicionarAsync(T entidade);
    Task AtualizarAsync(T entidade);
    Task DeletarAsync(int id);
}