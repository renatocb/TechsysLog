using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechsysLog.Infrastructure.External.ViaCep.Interfaces;

namespace TechsysLog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViaCepController : ControllerBase
    {
        private readonly IViaCepService _viaCepService;

        public ViaCepController(IViaCepService viaCepService)
        {
            _viaCepService = viaCepService;
        }

        [HttpGet("{cep}")]
        public async Task<IActionResult> GetAddressByCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
            {
                return BadRequest("CEP is required.");
            }

            var endereco = await _viaCepService.GetAddressByCepAsync(cep);

            if (endereco == null)
            {
                return NotFound("Address not found.");
            }

            return Ok(endereco);
        }
    }
}