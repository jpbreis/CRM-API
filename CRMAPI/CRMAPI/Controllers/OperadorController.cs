using CRMAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperadorController : ControllerBase
    {
        private readonly DataContext _context;

        public OperadorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        #region mock.data
        //public async Task<ActionResult<List<Operador>>> GetOperadores()
        //{
        //    return new List<Operador>
        //    {
        //        new Operador
        //        {
        //            Nome = "João Paulo",
        //            DataNascimento = "03/01/1992",
        //            Rg = "471946944",
        //            Cpf = "38810588851",
        //            Cep = "04180070",
        //            Logradouro = "Rua Francisco de Lima",
        //            Numero = "37",
        //            Bairro = "Jardim Maria Estela",
        //            Complemento = null,
        //            Pais = "Brasil",
        //            Estado = "SP",
        //            Cidade = "São Paulo",
        //            Celular = "5511994570716",
        //            Telefone = null,
        //            Email = "jpbreis92@gmail.com",
        //            EstadoCivil = "Solteiro",
        //            Dependentes = 1,
        //            Cargo = 1,
        //            Departamento = 1,
        //            Supervisor = 0,
        //            DataAdmissao = "01/04/2023",
        //            DataDemissao = null,
        //            Tipocontrato = 1,
        //            Regimetrabalho = 1,
        //            JornadaTrabalho = 1,
        //            Salario = 1
        //        }
        //    };
        #endregion
        public async Task<ActionResult<List<Operador>>> GetOperadores()
        {
            return Ok(await _context.Operadores.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<Operador>>> CreateOperador(Operador operador)
        {
            _context.Operadores.Add(operador);
            await _context.SaveChangesAsync();

            return Ok(await _context.Operadores.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Operador>>> UpdateOperador(Operador operador)
        {
            var dbOperador = await _context.Operadores.FindAsync(operador.IdOperador);
            if (dbOperador == null)
                return BadRequest("Operador não encontrtado.");

            dbOperador.Nome = operador.Nome;
            dbOperador.DataNascimento = operador.DataNascimento;
            dbOperador.Rg = operador.Rg;
            dbOperador.Cpf = operador.Cpf;
            dbOperador.Cep = operador.Cep;
            dbOperador.Logradouro = operador.Logradouro;
            dbOperador.Numero = operador.Numero;
            dbOperador.Bairro = operador.Bairro;
            dbOperador.Complemento = operador.Complemento;
            dbOperador.Estado = operador.Estado;
            dbOperador.Cidade = operador.Cidade;
            dbOperador.Pais = operador.Pais;
            dbOperador.Celular = operador.Celular;
            dbOperador.Telefone = operador.Telefone;
            dbOperador.Email = operador.Email;
            dbOperador.EstadoCivil = operador.EstadoCivil;
            dbOperador.Dependentes = operador.Dependentes;
            dbOperador.Cargo = operador.Cargo;
            dbOperador.Departamento = operador.Departamento;
            dbOperador.Supervisor = operador.Supervisor;
            dbOperador.DataAdmissao = operador.DataAdmissao;
            dbOperador.DataDemissao = operador.DataDemissao;
            dbOperador.Tipocontrato = operador.Tipocontrato;
            dbOperador.Regimetrabalho = operador.Regimetrabalho;
            dbOperador.JornadaTrabalho = operador.JornadaTrabalho;
            dbOperador.Salario = operador.Salario;

            await _context.SaveChangesAsync();

            return Ok(await _context.Operadores.ToListAsync());
        }

        [HttpDelete("{idOperador}")]
        public async Task<ActionResult<List<Operador>>> DeleteOperador(int idOperador)
        {
            var dbOperador = await _context.Operadores.FindAsync(idOperador);
            if (dbOperador == null)
                return BadRequest("Operador não encontrado.");

            _context.Operadores.Remove(dbOperador);
            await _context.SaveChangesAsync();

            return Ok(await _context.Operadores.ToListAsync());

        }
    }
}
