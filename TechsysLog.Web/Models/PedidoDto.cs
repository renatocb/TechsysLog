namespace TechsysLog.Web.Models
{
    public class PedidoDto
    {
        public int UsuarioId { get; set; }
        public string NumeroPedido { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }        
    }
}
