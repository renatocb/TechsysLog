using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using TechsysLog.Web.Models;
using TechsysLog.Web.Services;

namespace TechsysLog.Web
{
    public partial class PainelPedidos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    CarregarPedidos();
                }
            }
        }

        private async void CarregarPedidos()
        {
            var apiService = new ApiService();
            var pedidos = await apiService.ObterTodosPedidosAsync();

            // Serializa os pedidos para JSON e passa para o JavaScript
            var pedidosJson = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pedidos);

            ScriptManager.RegisterStartupScript(this, GetType(), "carregarPedidos", $"atualizarTabelaPedidos({pedidosJson});", true);
        }
    }
}

