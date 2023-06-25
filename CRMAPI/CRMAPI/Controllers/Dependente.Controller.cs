using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependenteController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public DependenteController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<DependenteModel>>> GetDependente(
            [FromQuery] int[]? ids,
            [FromQuery] int[]? idoperador,
            [FromQuery] string? nome,
            [FromQuery] string? genero,
            [FromQuery] string? cpf,
            [FromQuery] string? rg,
            [FromQuery] int[]? idparentesco,
            [FromQuery] string? cep,
            [FromQuery] string? logradouro,
            [FromQuery] string? numeroendereco,
            [FromQuery] string? bairro,
            [FromQuery] string? complemento,
            [FromQuery] string? estado,
            [FromQuery] string? cidade,
            [FromQuery] string? pais,
            [FromQuery] string? estadocivil,
            [FromQuery] string? celular,
            [FromQuery] string? telefone,
            [FromQuery] bool? ativo,
            [FromQuery] int[]? idscriacao,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datanascimento,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? datadesativadoBetween,
            [FromQuery] DateTime? datanascimentoBetween)
        {
            try
            {
                IQueryable<DependenteModel> query = _dbcontext.bdDependente;

                #region Filter
                bool hasFilter =
                    ids != null ||
                    idoperador != null ||
                    !string.IsNullOrEmpty(nome) ||
                    !string.IsNullOrEmpty(genero) ||
                    !string.IsNullOrEmpty(cpf) ||
                    !string.IsNullOrEmpty(rg) ||
                    idparentesco != null ||
                    !string.IsNullOrEmpty(cep) ||
                    !string.IsNullOrEmpty(logradouro) ||
                    !string.IsNullOrEmpty(numeroendereco) ||
                    !string.IsNullOrEmpty(bairro) ||
                    !string.IsNullOrEmpty(complemento) ||
                    !string.IsNullOrEmpty(estado) ||
                    !string.IsNullOrEmpty(cidade) ||
                    !string.IsNullOrEmpty(pais) ||
                    !string.IsNullOrEmpty(estadocivil) ||
                    !string.IsNullOrEmpty(celular) ||
                    !string.IsNullOrEmpty(telefone) ||
                    ativo != null ||
                    idscriacao != null ||
                    datacriacao != null ||
                    datadesativado != null ||
                    datanascimento != null ||
                    datacriacaoBetween != null ||
                    datadesativadoBetween != null ||
                    datanascimentoBetween != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdDependente
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdDependente
                        .Where(x => ids.Contains(x.IdDependente))
                        .ToListAsync();
                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null || datanascimento != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;
                    DependenteModel lead = new DependenteModel
                    {
                        DataCriacao = (DateTime)datacriacao,
                    };

                    if (datacriacao != null || datacriacaoBetween != null)
                    {
                        startDate = datacriacao.HasValue ? datacriacao.Value.Date : null;
                        endDate = datacriacaoBetween.HasValue ? datacriacaoBetween.Value.Date : null;
                    }
                    else if (datadesativado != null || datadesativadoBetween != null)
                    {
                        startDate = datadesativado.HasValue ? datadesativado.Value.Date : null;
                        endDate = datadesativadoBetween.HasValue ? datadesativadoBetween.Value.Date : null;
                    }
                    else if (datanascimento != null || datanascimentoBetween != null)
                    {
                        startDate = datanascimento.HasValue ? datanascimento.Value.Date : null;
                        endDate = datanascimentoBetween.HasValue ? datanascimentoBetween.Value.Date : null;
                    }

                    var dataFilter = datacriacao != null || datacriacaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<DependenteModel>(startDate, endDate, "DataCriacao")
                    : datadesativado!= null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<DependenteModel>(startDate, endDate, "DataDesativado")
                    : datanascimento != null || datanascimentoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<DependenteModel>(startDate, endDate, "DataNascimentoDependente")
                    : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                //nome
                if (!string.IsNullOrEmpty(nome))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("NomeDependente", nome);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //genero
                if (!string.IsNullOrEmpty(genero))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("Genero", genero);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //cpf
                if (!string.IsNullOrEmpty(cpf))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("CPFDependente", cpf);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //rg
                if (!string.IsNullOrEmpty(rg))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("RGDependente", rg);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //cep
                if (!string.IsNullOrEmpty(cep))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("CEP", cep);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //logradouro
                if (!string.IsNullOrEmpty(logradouro))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("Logradouro", logradouro);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //numeroendereco
                if (!string.IsNullOrEmpty(numeroendereco))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("NumeroEndereco", numeroendereco);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //bairro
                if (!string.IsNullOrEmpty(bairro))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("Bairro", bairro);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //complemento
                if (!string.IsNullOrEmpty(complemento))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("Complemento", complemento);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //estado
                if (!string.IsNullOrEmpty(estado))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("Estado", estado);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //cidade
                if (!string.IsNullOrEmpty(cidade))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("Cidade", cidade);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //pais
                if (!string.IsNullOrEmpty(pais))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("Pais", pais);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //estadocivil
                if (!string.IsNullOrEmpty(estadocivil))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("EstadoCivil", estadocivil);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //celular
                if (!string.IsNullOrEmpty(celular))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("Celular", celular);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //telefone
                if (!string.IsNullOrEmpty(telefone))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DependenteModel>("Telefone", telefone);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                //idoperador
                if (idoperador != null && idoperador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<DependenteModel>("IdOperador", idoperador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                //idparentesco
                if (idparentesco != null && idparentesco.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<DependenteModel>("IdParentesco", idparentesco);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                //idscriacao
                if (idscriacao != null && idscriacao.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<DependenteModel>("IdCriador", idscriacao);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region Bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdDependente;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Dependente", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<DependenteModel>>> CreateDependente(
            [FromQuery] int idoperador = 0,
            [FromQuery] string nome = "",
            [FromQuery] string genero = "",
            [FromQuery] string cpf = "",
            [FromQuery] string? rg = "",
            [FromQuery] int idparentesco = 0,
            [FromQuery] string cep = "",
            [FromQuery] string? logradouro = "",
            [FromQuery] string? numeroendereco = "",
            [FromQuery] string? bairro = "",
            [FromQuery] string? complemento = "",
            [FromQuery] string? estado = "",
            [FromQuery] string? cidade = "",
            [FromQuery] string pais = "",
            [FromQuery] string? estadocivil = "",
            [FromQuery] string celular = "",
            [FromQuery] string? telefone = "",
            [FromQuery] bool ativo = true,
            [FromQuery] int idscriacao = 0,
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] DateTime datanascimento = default,
            [FromQuery] DateTime? datadesativado = null)
        {
            try
            {
                if (idoperador != 0 &&
                    !string.IsNullOrEmpty(nome) &&
                    !string.IsNullOrEmpty(genero) &&
                    !string.IsNullOrEmpty(cpf) &&
                    !string.IsNullOrEmpty(cep) &&
                    !string.IsNullOrEmpty(pais) &&
                    !string.IsNullOrEmpty(celular) &&
                    idparentesco != 0)
                {
                    if (datacriacao != default)
                        datacriacao = DateTime.Today;

                    if (datanascimento != default)
                        datanascimento = DateTime.Today;

                    var newDependente = new DependenteModel
                    {
                        IdOperador = idoperador,
                        NomeDependente = nome,
                        DataNascimentoDependente = datanascimento,
                        Genero = genero,
                        CPFDependente = cpf,
                        RGDependente = string.IsNullOrEmpty(rg) ? null : rg,
                        CEP = cep,
                        Logradouro = string.IsNullOrEmpty(logradouro) ? null : logradouro,
                        NumeroEndereco = string.IsNullOrEmpty(numeroendereco) ? null : numeroendereco,
                        Bairro = string.IsNullOrEmpty(bairro) ? null : bairro,
                        Complemento = string.IsNullOrEmpty(complemento) ? null : complemento,
                        Estado = string.IsNullOrEmpty(estado) ? null : estado,
                        Cidade = string.IsNullOrEmpty(cidade) ? null : cidade,
                        Pais = pais,
                        EstadoCivil = string.IsNullOrEmpty(estadocivil) ? null : estadocivil,
                        Celular = celular,
                        Telefone = string.IsNullOrEmpty(telefone) ? null : telefone,
                        DataCriacao = datacriacao,
                        DataDesativado = string.IsNullOrEmpty(datadesativado.ToString()) ? null : datadesativado, 
                        Ativo = ativo,
                        IdCriador = idscriacao,
                        IdParentesco = idparentesco
                    };

                    _dbcontext.bdDependente.Add(newDependente);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdDependente.ToListAsync());
                }
                else
                {
                    string? props;
                    props = idoperador != 0 ? null : Concat.ConcatString("Operador Parente");
                    props = !string.IsNullOrEmpty(nome) ? null : Concat.ConcatString("Nome");
                    props = !string.IsNullOrEmpty(genero) ? null : Concat.ConcatString("Genero");
                    props = !string.IsNullOrEmpty(cpf) ? null : Concat.ConcatString("CPF");
                    props = !string.IsNullOrEmpty(cep) ? null : Concat.ConcatString("CEP");
                    props = !string.IsNullOrEmpty(pais) ? null : Concat.ConcatString("Pais");
                    props = !string.IsNullOrEmpty(celular) ? null : Concat.ConcatString("Celular");

                    return BadRequest(CRMMessages.PostMissingItems("Dependente", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Dependnte", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<List<DependenteModel>>> UpdateDepentende(DependenteModel dependente,
            [FromQuery] int? idoperador,
            [FromQuery] string? nome,
            [FromQuery] string? genero,
            [FromQuery] string? cpf,
            [FromQuery] string? rg,
            [FromQuery] int? idparentesco,
            [FromQuery] string? cep,
            [FromQuery] string? logradouro,
            [FromQuery] string? numeroendereco,
            [FromQuery] string? bairro,
            [FromQuery] string? complemento,
            [FromQuery] string? estado,
            [FromQuery] string? cidade,
            [FromQuery] string? pais,
            [FromQuery] string? estadocivil,
            [FromQuery] string? celular,
            [FromQuery] string? telefone,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datanascimento)
        {
            try
            {
                var dbDependente = await _dbcontext.bdDependente.FindAsync(dependente.IdDependente);
                if (dbDependente == null)
                    return BadRequest(CRMMessages.NotFind($"Dependente: {dependente.IdDependente} - {dependente.NomeDependente}"));
                else
                {
                    dbDependente.IdOperador = idoperador ?? dbDependente.IdOperador;
                    dbDependente.NomeDependente = nome ?? dbDependente.NomeDependente;
                    dbDependente.Genero = genero ?? dbDependente.Genero;
                    dbDependente.CPFDependente = cpf ?? dbDependente.CPFDependente;
                    dbDependente.RGDependente = rg ?? dbDependente.RGDependente;
                    dbDependente.CEP = cep ?? dbDependente.CEP;
                    dbDependente.Logradouro = logradouro ?? dbDependente.Logradouro;
                    dbDependente.NumeroEndereco = numeroendereco ?? dbDependente.NumeroEndereco;
                    dbDependente.Bairro = bairro ?? dbDependente.Bairro;
                    dbDependente.Complemento = complemento ?? dbDependente.Complemento;
                    dbDependente.Estado = estado ?? dbDependente.Estado;
                    dbDependente.Cidade = cidade ?? dbDependente.Cidade;
                    dbDependente.Pais = pais ?? dbDependente.Pais;
                    dbDependente.Estado = estadocivil ?? dbDependente.Estado;
                    dbDependente.Celular = celular ?? dbDependente.Celular;
                    dbDependente.Telefone = telefone ?? dbDependente.Telefone;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(dbDependente);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError($"Dependente: {dependente.IdDependente} - {dependente.NomeDependente}", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
