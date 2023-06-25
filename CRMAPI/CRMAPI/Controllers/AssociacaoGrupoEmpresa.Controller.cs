using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociacaoGrupoEmpresaController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public AssociacaoGrupoEmpresaController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<AssociacaoGrupoEmpresaModel>>> GetAssGrupoEmresa(
            [FromQuery] int[]? ids,
            [FromQuery] int[]? idgrupoempresa,
            [FromQuery] int[]? idempresa,
            [FromQuery] int[]? idcriador,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? datadesativadoBetween)
        {
            try
            {
                IQueryable<AssociacaoGrupoEmpresaModel> query = _dbcontext.bdAssociacaoGrupoEmpresa;

                #region Filter
                var hasFilter =
                    ids != null ||
                    idgrupoempresa != null ||
                    idempresa != null ||
                    idcriador != null ||
                    ativo != null ||
                    datacriacao != null ||
                    datadesativado != null ||
                    datacriacaoBetween != null ||
                    datadesativadoBetween != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdAssociacaoGrupo
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdAssociacaoGrupoEmpresa
                        .Where(x => ids.Contains(x.IdAssociacaoGrupo))
                        .ToListAsync();
                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    AssociacaoGrupoEmpresaModel model = new AssociacaoGrupoEmpresaModel
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
                    ? EndPointDataFilter.CreateDataFilter<AssociacaoGrupoEmpresaModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<AssociacaoGrupoEmpresaModel>(startDate, endDate, "DataDesativado") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<AssociacaoGrupoEmpresaModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                if (idempresa != null && idempresa.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<AssociacaoGrupoEmpresaModel>("IdEmpresa", idempresa);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                if (idgrupoempresa != null && idgrupoempresa.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<AssociacaoGrupoEmpresaModel>("IdGrupoEmpresa", idgrupoempresa);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region Bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdAssociacaoGrupoEmpresa;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Associação de Empresas", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<AssociacaoGrupoEmpresaModel>>> CreateAssociacaoGrupoEmpresa(
            [FromQuery] int? idgrupoempresa = 0,
            [FromQuery] int? idempresa = 0,
            [FromQuery] int idcriador = 0,
            [FromQuery] bool ativo = true,
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] DateTime? datadesativado = null)
        {
            try
            {
                if (idgrupoempresa != null && idempresa != null)
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Today;

                    var newAssociasaoEmpresa = new AssociacaoGrupoEmpresaModel
                    {
                        IdGrupoEmpresa = idgrupoempresa != 0 ? null : idgrupoempresa,
                        IdEmpresa = idempresa != 0 ? null : idempresa,
                        DataCriacao = datacriacao,
                        DataDesativado = !string.IsNullOrEmpty(datadesativado.ToString()) ? null : datadesativado,
                        Ativo = ativo,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdAssociacaoGrupoEmpresa.Add(newAssociasaoEmpresa);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdAssociacaoGrupoEmpresa.ToListAsync());
                }
                else
                {
                    string? props;
                    props = idgrupoempresa != 0 ? null : Concat.ConcatString("Grupo de Empresas");
                    props = idempresa != 0 ? null : Concat.ConcatString("Empresa");

                    return BadRequest(CRMMessages.PostMissingItems("Associação de Empresas", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Associação de Empresas", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<AssociacaoGrupoEmpresaModel>> UpdateAssocidadosEmpresa(AssociacaoGrupoEmpresaModel associacaoGrupoEmpresaModel,
            [FromQuery] int? idgrupoempresa,
            [FromQuery] int? idempresa,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? datadesativado)
        {
            var dbAssociasaoGrupo = await _dbcontext.bdAssociacaoGrupoEmpresa.FindAsync(associacaoGrupoEmpresaModel.IdAssociacaoGrupo);
            if (dbAssociasaoGrupo == null)
                return BadRequest(CRMMessages.NotFind("Assosiação de Grupo"));
            else
            {
                dbAssociasaoGrupo.IdGrupoEmpresa = idgrupoempresa ?? dbAssociasaoGrupo.IdGrupoEmpresa;
                dbAssociasaoGrupo.IdEmpresa = idempresa ?? dbAssociasaoGrupo.IdEmpresa;
                dbAssociasaoGrupo.DataDesativado = datadesativado ?? dbAssociasaoGrupo.DataDesativado;
                dbAssociasaoGrupo.Ativo = ativo ?? dbAssociasaoGrupo.Ativo;

                await _dbcontext.SaveChangesAsync();

                return Ok(dbAssociasaoGrupo);
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
