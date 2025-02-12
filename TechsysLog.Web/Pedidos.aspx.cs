using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechsysLog.Web.Models;
using TechsysLog.Web.Services;

namespace TechsysLog.Web
{
    public partial class Pedidos : Page
    {
        protected TextBox txtNumeroPedido;
        protected TextBox txtDescricao;
        protected TextBox txtValor;
        protected TextBox txtCep;
        protected TextBox txtRua;
        protected TextBox txtNumero;
        protected TextBox txtBairro;
        protected TextBox txtCidade;
        protected TextBox txtEstado;
        protected Label ErrorMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
            }
        }

        private string GetUserIdFromTicket()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                return ticket.Name; // Retorna o nome do usuário (email ou ID)
            }
            
            return null;
        }

        protected async void CadastrarPedidoButton_Click(object sender, EventArgs e)
        {           

            var usuarioId = GetUserIdFromTicket();

            if (usuarioId == null)
            {
                ErrorMessage.Text = "Erro ao obter o ID do usuário.";
                return;
            }

            decimal valor;
            if (!decimal.TryParse(txtValor.Text, out valor) || valor < 0)
            {
                valor = 0; // Valor padrão
            }

            var pedidoDto = new PedidoDto
            {
                UsuarioId = Convert.ToInt32(usuarioId),
                NumeroPedido = txtNumeroPedido.Text,
                Descricao = txtDescricao.Text,
                Valor = valor,
                Cep = txtCep.Text,
                Rua = txtRua.Text,
                Numero = txtNumero.Text,
                Bairro = txtBairro.Text,
                Cidade = txtCidade.Text,
                Estado = txtEstado.Text
            };

            var apiService = new ApiService();
            var (success, errorMessage, pedido) = await apiService.CadastrarPedidoAsync(pedidoDto);

            if (success)
            {
                // Exibir os dados do pedido em um modal
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", $"showPedidoModal('{pedido.NumeroPedido}', '{pedido.Descricao}', '{pedido.Valor}');", true);
            }
            else
            {             
                // Exibir mensagem de erro estilizada
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", $"showErrorAlert('{errorMessage}');", true);
            }
        }
    }
}
