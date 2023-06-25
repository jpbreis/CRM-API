using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public LoginController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<List<OperadorModel>>> Login(
            [FromQuery] string cpf = "",
            [FromQuery] string senha = "")
        {
            try
            {
                IQueryable<OperadorModel> query = _dbcontext.Operadores;

                if (string.IsNullOrEmpty(cpf) || string.IsNullOrEmpty(senha))
                    return BadRequest("Adicionar CPF e senha para realizar o login.");
                else
                {
                    var operador = await _dbcontext.Operadores.FirstOrDefaultAsync(o => o.CPFOperador == cpf && o.Senha == senha);

                    if (operador == null)
                        return BadRequest("Não existe um colaborador com esse CPF e senha.");
                    else
                        return Ok(operador);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Login", ex.ToString()));
            }
        }

        [HttpGet("ComparePassword")]
        public async Task<ActionResult<List<OperadorModel>>> ComparePassword(
            [FromQuery] string cpf = "")
        {
            if (string.IsNullOrEmpty(cpf))
                return BadRequest("CPF está vazio.");
            else
            {
                var pass = await _dbcontext.Operadores
                    .Where(o => o.CPFOperador == cpf)
                    .Select(o => o.Senha)
                    .FirstOrDefaultAsync();
                if (pass == null)
                    return BadRequest("CPF Invalido.");
                else
                    return Ok(pass);
            }
        }
    }
}
