using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Domain.Entities;
using TechsysLog.Domain.Interfaces;
using TechsysLog.Infrastructure.Data;

namespace TechsysLog.Infrastructure.Repositories
{
    public class NotificationRepositorio : Repositorio<Notification>, INotificationRepositorio
    {
        public NotificationRepositorio(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> ObterNotificacoesPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet.Where(n => n.UsuarioId == usuarioId).ToListAsync();
        }
    }
}
