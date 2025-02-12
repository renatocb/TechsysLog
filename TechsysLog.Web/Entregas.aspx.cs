using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechsysLog.Web.Models;
using TechsysLog.Web.Services;

namespace TechsysLog.Web
{
    public partial class Entregas : System.Web.UI.Page
    {
        protected TextBox NumeroPedidoTextBox;
        protected TextBox DataHoraEntregaTextBox;
        protected Label ErrorMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected async void RegistrarEntregaButton_Click(object sender, EventArgs e)
        {
            var entregaDto = new EntregaDto
            {                
                DataHoraEntrega = DateTime.Parse(DataHoraEntregaTextBox.Text)
            };

            var apiService = new ApiService();
            var result = await apiService.RegistrarEntregaAsync(entregaDto);

            if (result)
            {
                Response.Redirect("Entregas.aspx");
            }
            else
            {
                ErrorMessage.Text = "Erro ao registrar entrega.";
            }
        }
    }
}
