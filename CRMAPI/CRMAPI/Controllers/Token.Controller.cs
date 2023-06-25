using CRMAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Linq;
using CRMAPI.Utilities;
using CRMAPI.Models;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public TokenController(DataContext context)
        {
            _dbcontext = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<TK_NewOperador>>> CreateNewToken(
            TK_NewOperador tknewoperador,
            [FromQuery] string CPF = "",
            [FromQuery] string RG = "",
            [FromQuery] string Email = "3fake1true@gmail.com"
            )
        {
            IQueryable<TK_NewOperador> query = _dbcontext.tK_NewOperador;

            if (string.IsNullOrEmpty(CPF) || string.IsNullOrEmpty(RG))
                return BadRequest("Adicionar CPF e RG para solicitar nova adição de colaboradores.");
            else
            {
                var colaborador = await _dbcontext.Operadores.FirstOrDefaultAsync(o => o.CPFOperador == CPF);
                if (colaborador != null)
                    return BadRequest("Já existe um colaborador para esse CPF e RG.");
                else
                {
                    string randomString = GenerateRandomString.RandomToken(6);
                    var token = await _dbcontext.tK_NewOperador.FirstOrDefaultAsync(t => t.Token == tknewoperador.Token);
                    while (token != null)
                    {
                        randomString = GenerateRandomString.RandomToken(6);
                        token = await _dbcontext.tK_NewOperador.FirstOrDefaultAsync(t => t.Token == tknewoperador.Token);
                    }
                    var newToken = new TK_NewOperador
                    {
                        IdOperador = 0,
                        CpfNewOperador = CPF,
                        IdNewOperador = 0,
                        Token = randomString,
                        DataCriacao = DateTime.Now,
                        DataEnvio = DateTime.Now,
                        DataFinalizacao = null,
                        Email = Email
                    };

                    await _dbcontext.tK_NewOperador.AddAsync(newToken);
                    await _dbcontext.SaveChangesAsync();

                    SendEmail.SendEmailToNewOperador("3fake1true@gmail.com", randomString);

                    return Ok(newToken);
                }
            }
        }
    }
}
