using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechsysLog.Domain.Entities;

namespace TechsysLog.Domain.Interfaces
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}
