using System;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using TechsysLog.Web.Services;
using System.Web.UI.WebControls;

namespace TechsysLog.Web
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void LoginButton_Click(object sender, EventArgs e)
        {
            var email = txtEmail.Text;
            var senha = txtSenha.Text;

            var apiService = new ApiService();
            var result = await apiService.LoginUsuarioAsync(email, senha);

            if (result != null && result.Usuario.Id != 0)
            {
                // Remove o cookie antigo, se existir
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    var oldCookie = new HttpCookie(FormsAuthentication.FormsCookieName)
                    {
                        Expires = DateTime.Now.AddDays(-1) // Expira o cookie antigo
                    };
                    Response.Cookies.Add(oldCookie);
                }

                // Cria o ticket de autenticação
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,                              // Versão do ticket
                    result.Usuario.Id.ToString(),   // Nome do usuário (aqui você define o ID)
                    DateTime.Now,                   // Data de criação
                    DateTime.Now.AddMinutes(30),    // Data de expiração (30 minutos)
                    false,                          // Persistente (não manter o cookie após o fechamento do navegador)
                    result.Token.ToString()         // Dados adicionais (opcional)
                );

                // Criptografa o ticket
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                // Cria o cookie de autenticação
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                authCookie.HttpOnly = true; // Impede acesso ao cookie via JavaScript
                authCookie.Secure = true;   // Cookie só é enviado em conexões HTTPS
                Response.Cookies.Add(authCookie);

                // Armazena o token em um cookie separado
                HttpCookie tokenCookie = new HttpCookie("AuthToken", result.Token);
                tokenCookie.HttpOnly = true; // Impede acesso ao cookie via JavaScript
                tokenCookie.Secure = true;   // Cookie só é enviado em conexões HTTPS
                Response.Cookies.Add(tokenCookie);

                // Salva o token no localStorage
                string script = $@"
            <script>
                localStorage.setItem('token', '{result.Token}');
                window.location.href = 'Default.aspx'; // Redireciona para a página inicial
            </script>
        ";
                ClientScript.RegisterStartupScript(this.GetType(), "SaveToken", script);
            }
            else
            {
                ErrorMessage.Text = "Email ou senha incorretos.";
            }
        }
    }
}
