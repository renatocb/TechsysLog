namespace TechsysLog.Web.Models
{
    public class UsuarioDto
    {
        public string Token { get; set; }
        public Usuario Usuario { get; set; }
    }
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
