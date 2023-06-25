using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.Serialization;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public EmpresaController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<EmpresaModel>>> GetEmpresa(
            [FromQuery] int[]? ids,
            [FromQuery] string? nome,
            [FromQuery] string? cnpj,
            [FromQuery] bool? ativo,
            [FromQuery] int[]? idcriador,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? datadesativadoBetween)
        {
            try
            {
                IQueryable<EmpresaModel> query = _dbcontext.bdEmpresa;

                #region Filter
                var hasFilter =
                    ids != null ||
                    !string.IsNullOrEmpty(nome) ||
                    !string.IsNullOrEmpty(cnpj) ||
                    datacriacao != null ||
                    datadesativado != null ||
                    datacriacaoBetween != null ||
                    datadesativadoBetween != null ||
                    ativo != null ||
                    idcriador != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdEmpresa
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdEmpresa
                        .Where(x => ids.Contains(x.IdEmpresa))
                        .ToListAsync();
                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    EmpresaModel model = new EmpresaModel
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
                    ? EndPointDataFilter.CreateDataFilter<EmpresaModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<EmpresaModel>(startDate, endDate, "DataDesativado") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(nome))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<EmpresaModel>("NomeEmpresa", nome);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                if (!string.IsNullOrEmpty(nome))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<EmpresaModel>("CNPJEmpresa", cnpj);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<EmpresaModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region Bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdEmpresa;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Empresa", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<EmpresaModel>>> CreateEmpresa(
            [FromQuery] string nome = "",
            [FromQuery] string cnpj = "",
            [FromQuery] bool ativo = true,
            [FromQuery] int idcriador = 0,
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] DateTime? datadesativado = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(cnpj))
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Today;

                    var newEmpresa = new EmpresaModel
                    {
                        NomeEmpresa = nome,
                        CNPJEmpresa = cnpj,
                        DataCriacao = datacriacao,
                        DataDesativado = !string.IsNullOrEmpty(datadesativado.ToString()) ? null : datadesativado,
                        Ativo = ativo,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdEmpresa.Add(newEmpresa);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdEmpresa.ToListAsync());
                }
                else
                {
                    string? props;
                    props = !string.IsNullOrEmpty(nome) ? null : Concat.ConcatString("Nome");
                    props = !string.IsNullOrEmpty(cnpj) ? null : Concat.ConcatString("CNPJ");

                    return BadRequest(CRMMessages.PostMissingItems("Empresa", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Empresa", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<EmpresaModel>> UpdateEmpresa(EmpresaModel empresaModel,
            [FromQuery] string? nome,
            [FromQuery] string? cnpj,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var dbEmpresa = await _dbcontext.bdEmpresa.FindAsync(empresaModel.IdEmpresa);
                if (dbEmpresa == null)
                    return BadRequest(CRMMessages.NotFind("Empresa"));
                else
                {
                    dbEmpresa.NomeEmpresa = nome ?? dbEmpresa.NomeEmpresa;
                    dbEmpresa.CNPJEmpresa = cnpj ?? dbEmpresa.CNPJEmpresa;
                    dbEmpresa.DataDesativado = datadesativado ?? dbEmpresa.DataDesativado;
                    dbEmpresa.Ativo = ativo ?? dbEmpresa.Ativo;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(dbEmpresa);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Empresa", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
