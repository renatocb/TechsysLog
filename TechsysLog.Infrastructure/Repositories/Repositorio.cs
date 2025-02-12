using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Domain.Entities;
using TechsysLog.Domain.Interfaces;
using TechsysLog.Infrastructure.Data;

namespace TechsysLog.Infrastructure.Repositories;

public class Repositorio<T> : IRepositorio<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repositorio(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> ObterTodosAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> ObterTodosAsync(params Expression<Func<T, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();

        // Inclui as entidades relacionadas
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public async Task<T?> ObterPorEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(e => EF.Property<string>(e, "Email") == email);
    }

    public async Task<T?> ObterPorIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> ObterPorIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();

        // Inclui as entidades relacionadas
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<T?> ObterPorNumeroAsync(string numeroPedido)
    {
        return await _dbSet.FirstOrDefaultAsync(e => EF.Property<string>(e, "NumeroPedido") == numeroPedido);
    }

    public async Task AdicionarAsync(T entidade)
    {
        await _dbSet.AddAsync(entidade);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(T entidade)
    {
        _dbSet.Update(entidade);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(int id)
    {
        var entidade = await _dbSet.FindAsync(id);
        if (entidade != null)
        {
            _dbSet.Remove(entidade);
            await _context.SaveChangesAsync();
        }
    }
}
