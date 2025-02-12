using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechsysLog.Application.DTOs;

public class UsuarioDto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
}