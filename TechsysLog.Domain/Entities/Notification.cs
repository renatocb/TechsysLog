using TechsysLog.Domain.Enums;

namespace TechsysLog.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public TipoNotificacao Tipo { get; set; }
        public required string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }
}
