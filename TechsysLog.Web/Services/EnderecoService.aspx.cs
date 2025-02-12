using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using TechsysLog.Web.Models;

namespace TechsysLog.Web.Services
{
    public partial class EnderecoService : System.Web.UI.Page
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static async Task<EnderecoDto> ObterEnderecoPorCep(string cep)
        {
            var apiService = new ApiService();
            var endereco = await apiService.ObterEnderecoPorCepAsync(cep);
            return endereco;
        }
    }
}