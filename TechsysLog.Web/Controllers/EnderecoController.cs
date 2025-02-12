using System.Threading.Tasks;
using System.Web.Mvc;
using TechsysLog.Web.Models;
using TechsysLog.Web.Services;

namespace TechsysLog.Web.Controllers
{
    public class EnderecoController : Controller
    {
        private readonly ApiService _apiService;

        public EnderecoController()
        {
            _apiService = new ApiService();
        }

        [HttpGet]
        public async Task<ActionResult> ObterEnderecoPorCep(string cep)
        {
            var endereco = await _apiService.ObterEnderecoPorCepAsync(cep);
            if (endereco == null)
            {
                return HttpNotFound();
            }
            return Json(endereco, JsonRequestBehavior.AllowGet);
        }
    }
}