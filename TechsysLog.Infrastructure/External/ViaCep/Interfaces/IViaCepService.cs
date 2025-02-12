using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Infrastructure.External.ViaCep.Models;

namespace TechsysLog.Infrastructure.External.ViaCep.Interfaces
{
    public interface IViaCepService
    {
        Task<ViaCepResponse> GetAddressByCepAsync(string cep);
    }
}
