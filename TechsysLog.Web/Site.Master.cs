using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TechsysLog.Web
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            // Faz o logout
            FormsAuthentication.SignOut();

            // Redireciona para a página de login
            Response.Redirect("Login.aspx");
        }
    }
}