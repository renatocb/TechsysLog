using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechsysLog.Web.Models;
using TechsysLog.Web.Services;

namespace TechsysLog.Web
{
    public partial class Register : Page
    {
        protected TextBox txtNome;
        protected TextBox txtEmail;
        protected TextBox txtSenha;
        protected Label ErrorMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void RegisterButton_Click(object sender, EventArgs e)
        {
            var registroUsuarioDto = new RegistroUsuarioDto
            {
                Nome = txtNome.Text,
                Email = txtEmail.Text,
                Senha = txtSenha.Text
            };

            var apiService = new ApiService();
            var (success, errorMessage, usuario) = await apiService.RegisterUsuarioAsync(registroUsuarioDto);

            if (success)
            {             
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", $"showUserModal('{usuario.Nome}', '{usuario.Email}', '{usuario.Id}');", true);
            }        
            else
            {                
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", $"showErrorAlert('{errorMessage}');", true);
            }
        }
    }
}