using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrupoEmpresaController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public GrupoEmpresaController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<GrupoEmpresaModel>>> GetGrupoEmpresa(
            [FromQuery] int[]? ids,
            [FromQuery] string? nome,
            [FromQuery] bool? ativo,
            [FromQuery] int[]? idcriador,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? datadesativadoBetween)
        {
            try
            {
                IQueryable<GrupoEmpresaModel> query = _dbcontext.bdGrupoEmpresa;

                #region Filter
                var hasFilter =
                    ids != null ||
                    !string.IsNullOrEmpty(nome) ||
                    datacriacao != null ||
                    datadesativado != null ||
                    datacriacaoBetween != null ||
                    datadesativadoBetween != null ||
                    ativo != null ||
                    idcriador != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdGrupoEmpresa
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdGrupoEmpresa
                        .Where(x => ids.Contains(x.IdGrupoEmpresa))
                        .ToListAsync();
                    return Ok(TabeleaID);
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<GrupoEmpresaModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    GrupoEmpresaModel model = new GrupoEmpresaModel
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
                    ? EndPointDataFilter.CreateDataFilter<GrupoEmpresaModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<GrupoEmpresaModel>(startDate, endDate, "DataDesativado") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(nome))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<GrupoEmpresaModel>("NomeGrupo", nome);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdGrupoEmpresa;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Grupo de Empresas", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<GrupoEmpresaModel>>> CreateGrupoEmpresa(
            [FromQuery] string nome = "",
            [FromQuery] bool ativo = true,
            [FromQuery] int idcriador = 0,
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] DateTime? datadesativado = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(nome))
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Today;

                    var newGrupoEmpresa = new GrupoEmpresaModel
                    {
                        NomeGrupo = nome,
                        DataCriacao = datacriacao,
                        DataDesativado = !string.IsNullOrEmpty(datadesativado.ToString()) ? null : datadesativado,
                        Ativo = ativo,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdGrupoEmpresa.Add(newGrupoEmpresa);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdGrupoEmpresa.ToListAsync());
                }
                else
                {
                    string props = !string.IsNullOrEmpty(nome) ? null : Concat.ConcatString("Nome");

                    return BadRequest(CRMMessages.PostMissingItems("Grupo de Empresas", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Grupo de Empresas", ex.ToString()));
            }
        }
        #endregion
        
        #region HttpPut
        #endregion
        
        #region HttpDelete
        #endregion
    }
}
