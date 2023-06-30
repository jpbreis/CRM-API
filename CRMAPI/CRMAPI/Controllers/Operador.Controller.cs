using CRMAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Net.Mail;
using System;
using CRMAPI.Utilities;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.IdentityModel.Tokens;
using CRMAPI.Models;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperadorController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public OperadorController(DataContext context)
        {
            _dbcontext = context;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<OperadorModel>>> GetBuscaOp(
            [FromQuery] int[]? ids,
            [FromQuery] string? nome,
            [FromQuery] string? cpf,
            [FromQuery] string? email,
            [FromQuery] string? celular,
            [FromQuery] string? telefone,
            [FromQuery] string? genero,
            [FromQuery] decimal[]? salario,
            [FromQuery] string? senha,
            [FromQuery] int[]? idsempresa,
            [FromQuery] int[]? idsgrupoempresa,
            [FromQuery] int[]? idscargo,
            [FromQuery] int[]? idsdepartamento,
            [FromQuery] int[]? idsgestao,
            [FromQuery] int[]? idscontratotrabalho,
            [FromQuery] int[]? idsjornadatrabalho,
            [FromQuery] int[]? idsnivelacesso,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? datanasc,
            [FromQuery] DateTime? dataadmissao,
            [FromQuery] DateTime? datademissao,
            [FromQuery] DateTime? datanascBetween,
            [FromQuery] DateTime? dataadmissaoBetween,
            [FromQuery] DateTime? datademissaoBetween,
            [FromQuery] int[]? idscriacao)
        {
            try
            {
                IQueryable<OperadorModel> query = _dbcontext.Operadores;

                #region Filter
                bool hasFilter =
                    ids != null ||
                    nome != null ||
                    cpf != null ||
                    email != null ||
                    celular != null ||
                    telefone != null ||
                    genero != null ||
                    salario != null ||
                    senha != null ||
                    idsempresa != null ||
                    idsgrupoempresa != null ||
                    idscargo != null ||
                    idsdepartamento != null ||
                    idsgestao != null ||
                    idscontratotrabalho != null ||
                    idsjornadatrabalho != null ||
                    idsnivelacesso != null ||
                    ativo != null ||
                    idscriacao != null ||
                    datanasc != null ||
                    dataadmissao != null ||
                    datademissao != null;
                

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdOperador
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.Operadores
                        .Where(x => ids.Contains(x.IdOperador))
                        .ToListAsync();

                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (dataadmissao != null || datademissao != null || datanasc != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    OperadorModel model = new OperadorModel
                    {
                        DataAdmissao = (DateTime)dataadmissao,
                        DataNascimentoOperador = (DateTime)datanasc
                    };

                    if (dataadmissao != null || dataadmissaoBetween != null)
                    {
                        startDate = dataadmissao.HasValue ? dataadmissao.Value.Date : null;
                        endDate = dataadmissaoBetween.HasValue ? dataadmissaoBetween.Value.Date : null;
                    }
                    else if (datademissao != null || datademissaoBetween != null)
                    {
                        startDate = datademissao.HasValue ? datademissao.Value.Date : null;
                        endDate = datademissaoBetween.HasValue ? datademissaoBetween.Value.Date : null;
                    }
                    else if (datanasc != null || datanascBetween != null)
                    {
                        startDate = datanasc.HasValue ? datanasc.Value.Date : null;
                        endDate = datanascBetween.HasValue ? datanascBetween.Value.Date : null;
                    }

                    var dataFilter = dataadmissao != null || dataadmissaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<OperadorModel>(startDate, endDate, "DataAdmissao")
                    : datademissao != null || datademissaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<OperadorModel>(startDate, endDate, "DataDemissao")
                    : datanasc != null || datanascBetween != null
                    ? EndPointDataFilter.CreateDataFilter<OperadorModel>(startDate, endDate, "DataNascimentoOperador")
                    : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                        if (!string.IsNullOrEmpty(nome))
                        {
                            var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadorModel>("NomeOperador", nome);

                            if (stringFilter != null)
                                query = query.Where(stringFilter).AsQueryable();
                        }

                        if (!string.IsNullOrEmpty(cpf))
                        {
                            var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadorModel>("CPFOperador", cpf);

                            if (stringFilter != null)
                                query = query.Where(stringFilter).AsQueryable();
                        }

                        if (!string.IsNullOrEmpty(email))
                        {
                            var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadorModel>("EmailOperador", email);

                            if (stringFilter != null)
                                query = query.Where(stringFilter).AsQueryable();
                        }

                        if (!string.IsNullOrEmpty(celular))
                        {
                            var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadorModel>("Celular", celular);

                            if (stringFilter != null)
                                query = query.Where(stringFilter).AsQueryable();
                        }

                        if (!string.IsNullOrEmpty(telefone))
                        {
                            var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadorModel>("Telefone", telefone);

                            if (stringFilter != null)
                                query = query.Where(stringFilter).AsQueryable();
                        }

                        if (!string.IsNullOrEmpty(genero))
                        {
                            var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadorModel>("Genero", genero);

                            if (stringFilter != null)
                                query = query.Where(stringFilter).AsQueryable();
                        }

                #endregion

                #region Decimal
                if (salario != null && salario.Length > 0)
                {
                    var decimalFilter = EndPointDecimalFilter.CreateDoublePropertyFilter<OperadorModel>("Salario", salario);
                    if (decimalFilter != null)
                        query = query.Where(decimalFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idsempresa != null && idsempresa.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<OperadorModel>("IdEmpresa", idsempresa);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                if (idsgrupoempresa != null && idsgrupoempresa.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<OperadorModel>("IdGrupoEmpresa", idsgrupoempresa);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                if (idscargo != null && idscargo.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<OperadorModel>("IdCargo", idscargo);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                if (idsdepartamento != null && idsdepartamento.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<OperadorModel>("IdDepartamento", idsdepartamento);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                if (idsgestao != null && idsgestao.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<OperadorModel>("IdGestao", idsgestao);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                if (idscontratotrabalho != null && idscontratotrabalho.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<OperadorModel>("IdContratoTrabalho", idscontratotrabalho);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                if (idsjornadatrabalho != null && idsjornadatrabalho.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<OperadorModel>("IdJornadaTrabalho", idsjornadatrabalho);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                if (idsnivelacesso != null && idsnivelacesso.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<OperadorModel>("IdNivelAcesso", idsnivelacesso);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                if (idscriacao != null && idscriacao.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<OperadorModel>("IdCriador", idscriacao);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.Operadores;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Operador".GetError(ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<OperadorModel>>> CreateOperador(OperadorModel operador,
            [FromQuery] string nome = "",
            [FromQuery] string cpf = "",
            [FromQuery] string? email = "",
            [FromQuery] string celular = "",
            [FromQuery] string? telefone = "",
            [FromQuery] string genero = "",
            [FromQuery] decimal salario = 0,
            [FromQuery] string senha = "",
            [FromQuery] int idsempresa = 0,
            [FromQuery] int? idsgrupoempresa = 0,
            [FromQuery] int idscargo = 0,
            [FromQuery] int idsdepartamento = 0,
            [FromQuery] int idsgestao = 0,
            [FromQuery] int idscontratotrabalho = 0,
            [FromQuery] int idsjornadatrabalho = 0,
            [FromQuery] int idsnivelacesso = 0,
            [FromQuery] bool ativo = false,
            [FromQuery] DateTime datanasc = default,
            [FromQuery] DateTime dataadmissao = default,
            [FromQuery] DateTime? datademissao = null,
            [FromQuery] int idscriacao = 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(nome) &&
                !string.IsNullOrEmpty(cpf) &&
                !string.IsNullOrEmpty(email) &&
                !string.IsNullOrEmpty(celular) &&
                !string.IsNullOrEmpty(genero) &&
                !string.IsNullOrEmpty(senha) &&
                idscargo != 0 &&
                idsdepartamento != 0 &&
                idsgestao != 0 &&
                idscontratotrabalho != 0 &&
                idsjornadatrabalho != 0 &&
                idsnivelacesso != 0)
                {
                    if (datanasc != default)
                        datanasc = DateTime.Today;

                    if (dataadmissao != default)
                        dataadmissao = DateTime.Today;

                    var newOperador = new OperadorModel
                    {
                        NomeOperador = nome,
                        DataNascimentoOperador = datanasc,
                        CPFOperador = cpf,
                        EmailOperador = email,
                        Celular = celular,
                        Telefone = string.IsNullOrEmpty(telefone) ? null : telefone,
                        DataAdmissao = string.IsNullOrEmpty(dataadmissao.ToString()) ? DateTime.Now : dataadmissao,
                        DataDemissao = string.IsNullOrEmpty(datademissao.ToString()) ? null : datademissao,
                        Genero = genero,
                        Salario = salario,
                        Senha = senha,
                        IdEmpresa = idsempresa,
                        IdGrupoEmpresa = idsgrupoempresa == 0 ? null : idsgrupoempresa,
                        IdCargo = idscargo,
                        IdDepartamento = idsdepartamento,
                        IdGestao = idsgestao,
                        IdContratoTrabalho = idscontratotrabalho,
                        IdJornadaTrabalho = idsjornadatrabalho,
                        IdNivelAcesso = idsnivelacesso,
                        Ativo = ativo,
                        IdCriador = idscriacao
                    };

                    _dbcontext.Operadores.Add(newOperador);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.Operadores.ToListAsync());
                }
                else
                {
                    string? props;
                    props = !string.IsNullOrEmpty(nome) ? null : Concat.ConcatString("Nome");
                    props = !string.IsNullOrEmpty(cpf) ? null : Concat.ConcatString("Cpf");
                    props = !string.IsNullOrEmpty(email) ? null : Concat.ConcatString("Email");
                    props = !string.IsNullOrEmpty(celular) ? null : Concat.ConcatString("Celular");
                    props = !string.IsNullOrEmpty(genero) ? null : Concat.ConcatString("Genero");
                    props = !string.IsNullOrEmpty(senha) ? null : Concat.ConcatString("Senha");
                    props = idscargo != 0 ? null : Concat.ConcatString("Cargo");
                    props = idsdepartamento != 0 ? null : Concat.ConcatString("Departamento");
                    props = idsgestao != 0 ? null : Concat.ConcatString("Gestor");
                    props = idscontratotrabalho != 0 ? null : Concat.ConcatString("Contrato De Trabalho");
                    props = idsjornadatrabalho != 0 ? null : Concat.ConcatString("Jornada De Trabalho");
                    props = idsnivelacesso != 0 ? null : Concat.ConcatString("Nivel de Acesso");

                    return BadRequest(CRMMessages.PostMissingItems("Operador", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Operador", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<List<OperadorModel>>> UpdateOperador(OperadorModel operador,
            [FromQuery] string? nome,
            [FromQuery] string? cpf,
            [FromQuery] string? email,
            [FromQuery] string? celular,
            [FromQuery] string? telefone,
            [FromQuery] string? genero,
            [FromQuery] decimal? salario,
            [FromQuery] string? senha,
            [FromQuery] int? idempresa,
            [FromQuery] int? idgrupoempresa,
            [FromQuery] int? idcargo,
            [FromQuery] int? iddepartamento,
            [FromQuery] int? idgestao,
            [FromQuery] int? idcontratotrabalho,
            [FromQuery] int? idjornadatrabalho,
            [FromQuery] int? idnivelacesso,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? datanasc,
            [FromQuery] DateTime? dataadmissao,
            [FromQuery] DateTime? datademissao)
        {
            try
            {
                /*
                VERIFICAR COMO REALIZAR UPDATE EM LOTE (UPDATE Tabela SET ... WHERE IdOperador IN (Ids...))
                */
                var dbOperador = await _dbcontext.Operadores.FindAsync(operador.IdOperador);
                if (dbOperador == null)
                    return BadRequest("Operador".NotFind());
                else
                {
                    dbOperador.NomeOperador = nome ?? dbOperador.NomeOperador;
                    dbOperador.CPFOperador = cpf ?? dbOperador.CPFOperador;
                    dbOperador.EmailOperador = email ?? dbOperador.EmailOperador;
                    dbOperador.Celular = celular ?? dbOperador.Celular;
                    dbOperador.Telefone = telefone ?? dbOperador.Telefone;
                    dbOperador.Genero = genero ?? dbOperador.Genero;
                    dbOperador.Salario = salario ?? dbOperador.Salario;
                    dbOperador.Senha = senha ?? dbOperador.Senha;
                    dbOperador.IdEmpresa = idempresa ?? dbOperador.IdEmpresa;
                    dbOperador.IdGrupoEmpresa = idgrupoempresa ?? dbOperador.IdGrupoEmpresa;
                    dbOperador.IdCargo = idcargo ?? dbOperador.IdCargo;
                    dbOperador.IdDepartamento = iddepartamento ?? dbOperador.IdDepartamento;
                    dbOperador.IdGestao = idgestao ?? dbOperador.IdGestao;
                    dbOperador.IdContratoTrabalho = idcontratotrabalho ?? dbOperador.IdContratoTrabalho;
                    dbOperador.IdJornadaTrabalho = idjornadatrabalho ?? dbOperador.IdJornadaTrabalho;
                    dbOperador.IdNivelAcesso = idnivelacesso ?? dbOperador.IdNivelAcesso;
                    dbOperador.Ativo = ativo ?? dbOperador.Ativo;
                    dbOperador.DataNascimentoOperador = datanasc ?? dbOperador.DataNascimentoOperador;
                }

                await _dbcontext.SaveChangesAsync();

                return Ok(dbOperador);
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError($"Operador: {operador.IdOperador} - {operador.NomeOperador}", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        //[HttpDelete("{idOperador}")]
        //public async Task<ActionResult<List<Operador>>> DeleteOperador(int idOperador)
        //{
        //    try
        //    {
        //        var dbOperador = await _dbcontext.Operadores.FindAsync(idOperador);
        //        if (dbOperador == null)
        //            return BadRequest(CRMMessages.NotFind("Operador"));

        //        _dbcontext.Operadores.Remove(dbOperador);
        //        await _dbcontext.SaveChangesAsync();

        //        return Ok("");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("");
        //    }
        //}
        #endregion
    }
}
