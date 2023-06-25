using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Numerics;
using System.Runtime.ConstrainedExecution;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperadoresComplController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public OperadoresComplController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<OperadoresComplModel>>> GetOperadoresCompl(
            [FromQuery] int[]? ids,
            [FromQuery] int[]? idoperador,
            [FromQuery] string? rg,
            [FromQuery] string? cep,
            [FromQuery] string? logradouro,
            [FromQuery] string? numeroendereco,
            [FromQuery] string? bairro,
            [FromQuery] string? complemento,
            [FromQuery] string? estado,
            [FromQuery] string? cidade,
            [FromQuery] string? pais,
            [FromQuery] string? estadocivil,
            [FromQuery] bool? ativo,
            [FromQuery] int[]? idcriador,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? datadesativadoBetween)
        {
            try
            {
                IQueryable<OperadoresComplModel> query = _dbcontext.OperadoresCompl;

                #region Filter
                var hasFilter =
                    ids != null ||
                    idoperador != null ||
                    !string.IsNullOrEmpty(rg) ||
                    !string.IsNullOrEmpty(cep) ||
                    !string.IsNullOrEmpty(logradouro) ||
                    !string.IsNullOrEmpty(numeroendereco) ||
                    !string.IsNullOrEmpty(bairro) ||
                    !string.IsNullOrEmpty(complemento) ||
                    !string.IsNullOrEmpty(estado) ||
                    !string.IsNullOrEmpty(cidade) ||
                    !string.IsNullOrEmpty(pais) ||
                    !string.IsNullOrEmpty(estadocivil) ||
                    datacriacao != null ||
                    datadesativado != null ||
                    ativo != null ||
                    idcriador != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    OperadoresComplModel model = new OperadoresComplModel
                    {
                        DataCriacao = (DateTime)datacriacao
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

                    var dataFilter = datacriacao != null || datacriacaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<OperadoresComplModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<OperadoresComplModel>(startDate, endDate, "DataDesativado") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                //cep
                if (!string.IsNullOrEmpty(cep))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadoresComplModel>("CEP", cep);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //logradouro
                if (!string.IsNullOrEmpty(logradouro))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadoresComplModel>("Logradouro", logradouro);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //numeroendereco
                if (!string.IsNullOrEmpty(numeroendereco))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadoresComplModel>("NumeroEndereco", numeroendereco);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //bairro
                if (!string.IsNullOrEmpty(bairro))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadoresComplModel>("Bairro", bairro);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //complemento
                if (!string.IsNullOrEmpty(complemento))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadoresComplModel>("Complemento", complemento);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //estado
                if (!string.IsNullOrEmpty(estado))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadoresComplModel>("Estado", estado);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //cidade
                if (!string.IsNullOrEmpty(cidade))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadoresComplModel>("Cidade", cidade);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //pais
                if (!string.IsNullOrEmpty(pais))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadoresComplModel>("Pais", pais);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //OutroMidia
                if (!string.IsNullOrEmpty(estadocivil))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<OperadoresComplModel>("EstadoCivil", estadocivil);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<OperadoresComplModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.OperadoresCompl;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion  

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Complemento de Operadores", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<OperadoresComplModel>>> CretaeOperadorCompl(
            [FromQuery] int idoperador = 0,
            [FromQuery] string? rg = "",
            [FromQuery] string cep = "",
            [FromQuery] string? logradouro = "",
            [FromQuery] string? numeroendereco = "",
            [FromQuery] string? bairro = "",
            [FromQuery] string? complemento = "",
            [FromQuery] string? estado = "",
            [FromQuery] string? cidade = "",
            [FromQuery] string pais = "",
            [FromQuery] string? estadocivil = "",
            [FromQuery] bool ativo = true,
            [FromQuery] int idcriador = 0,
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] DateTime? datadesativado = null)
        {
            try
            {
                if (idoperador != 0 &&
                    !string.IsNullOrEmpty(cep) &&
                    !string.IsNullOrEmpty(pais))
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Today; 

                    var newOperadoresCompl = new OperadoresComplModel
                    {
                        IdOperador = idoperador,
                        RGOperador = !string.IsNullOrEmpty(rg) ? null : rg,
                        CEP = cep,
                        Logradouro = !string.IsNullOrEmpty(logradouro) ? null : logradouro,
                        NumeroEndereco = !string.IsNullOrEmpty(numeroendereco) ? null : numeroendereco,
                        Bairro = !string.IsNullOrEmpty(bairro) ? null : bairro,
                        Complemento = !string.IsNullOrEmpty(complemento) ? null : complemento,
                        Estado = !string.IsNullOrEmpty(estado) ? null : estado,
                        Cidade = !string.IsNullOrEmpty(cidade) ? null : cidade,
                        Pais = pais,
                        EstadoCivil = !string.IsNullOrEmpty(estadocivil) ? null : estadocivil,
                        DataCriacao = datacriacao,
                        DataDesativado = !string.IsNullOrEmpty(datacriacao.ToString()) ? null : datacriacao,
                        Ativo = ativo,
                        IdCriador = idcriador,
                    };

                    _dbcontext.OperadoresCompl.Add(newOperadoresCompl);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.OperadoresCompl.ToListAsync());
                }
                else
                {
                    string? props;
                    props = idoperador != 0 ? null : Concat.ConcatString("Operador Principal");
                    props = !string.IsNullOrEmpty(cep) ? null : Concat.ConcatString("CEP");
                    props = !string.IsNullOrEmpty(pais) ? null : Concat.ConcatString("Pais");

                    return BadRequest(CRMMessages.PostMissingItems("Complemento de Operadore", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Complemento de Operadore", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<OperadoresComplModel>> UpdateOperadoresCompl(OperadoresComplModel operadoresComplModel,
            [FromQuery] int? idoperador,
            [FromQuery] string? rg,
            [FromQuery] string? cep,
            [FromQuery] string? logradouro,
            [FromQuery] string? numeroendereco,
            [FromQuery] string? bairro,
            [FromQuery] string? complemento,
            [FromQuery] string? estado,
            [FromQuery] string? cidade,
            [FromQuery] string? pais,
            [FromQuery] string? estadocivil,
            [FromQuery] bool? ativo,
            [FromQuery] int? idcriador,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var dbOpCompl = await _dbcontext.OperadoresCompl.FindAsync(operadoresComplModel.IdOperador);
                if (dbOpCompl == null)
                    return BadRequest(CRMMessages.NotFind("Complemento de Operadore"));
                else
                {
                    dbOpCompl.IdOperador = idoperador ?? dbOpCompl.IdOperador;
                    dbOpCompl.RGOperador = rg ?? dbOpCompl.RGOperador;
                    dbOpCompl.CEP = cep ?? dbOpCompl.CEP;
                    dbOpCompl.Logradouro = logradouro ?? dbOpCompl.Logradouro;
                    dbOpCompl.NumeroEndereco = numeroendereco ?? dbOpCompl.NumeroEndereco;
                    dbOpCompl.Bairro = bairro ?? dbOpCompl.Bairro;
                    dbOpCompl.Complemento = complemento ?? dbOpCompl.Complemento;
                    dbOpCompl.Estado = estado ?? dbOpCompl.Estado;
                    dbOpCompl.Cidade = cidade ?? dbOpCompl.Cidade;
                    dbOpCompl.Pais = pais ?? dbOpCompl.Pais;
                    dbOpCompl.EstadoCivil = estadocivil ?? dbOpCompl.EstadoCivil;
                    dbOpCompl.DataDesativado = datadesativado ?? dbOpCompl.DataDesativado;
                    dbOpCompl.Ativo = ativo ?? dbOpCompl.Ativo;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(dbOpCompl);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Complemento de Operadore", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
