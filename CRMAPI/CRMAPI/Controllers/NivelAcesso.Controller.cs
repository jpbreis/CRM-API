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
    public class NivelAcessoController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public NivelAcessoController(DataContext context)
        {
            _dbcontext = context;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<NivelAcessoModel>>> GetNivelAcesso(
            [FromQuery] int[]? ids,
            [FromQuery] string? descricao,
            [FromQuery] bool? ativo,
            [FromQuery] int[]? idscriacao,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? datadesativadoBetween)
        {
            try
            {
                IQueryable<NivelAcessoModel> query = _dbcontext.bdNivelAcesso;

                #region Filter
                bool hasFilter =
                    ids != null ||
                    !string.IsNullOrEmpty(descricao) ||
                    ativo != null ||
                    idscriacao != null ||
                    datacriacao != null ||
                    datadesativado != null ||
                    datacriacaoBetween != null ||
                    datadesativadoBetween != null;


                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdNivelAcesso
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdNivelAcesso
                        .Where(x => ids.Contains(x.IdNivelAcesso))
                        .ToListAsync();
                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;
                    NivelAcessoModel model = new NivelAcessoModel
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

                    var dataFilter = datacriacao != null || datacriacaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<NivelAcessoModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<NivelAcessoModel>(startDate, endDate, "DataDesativado") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(descricao))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<NivelAcessoModel>("DescricaoNivel", descricao);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idscriacao != null && idscriacao.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<NivelAcessoModel>("IdCriador", idscriacao);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region Bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdNivelAcesso;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Nivel de Acesso", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<NivelAcessoModel>>> CreateNivelAcesso(
            [FromQuery] string descricao = "",
            [FromQuery] bool ativo = true,
            [FromQuery] int idscriacao = 0,
            [FromQuery] DateTime datacriacao = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(descricao))
                {
                    var newNivelAcesso = new NivelAcessoModel
                    {
                        DescricaoNivel = descricao,
                        Ativo = ativo,
                        IdCriador = idscriacao,
                        DataCriacao = string.IsNullOrEmpty(datacriacao.ToString()) ? DateTime.Today : datacriacao
                    };

                    _dbcontext.bdNivelAcesso.Add(newNivelAcesso);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdNivelAcesso.ToListAsync());
                }
                else
                {
                    string? props = !string.IsNullOrEmpty(descricao) ? null : Concat.ConcatString("Descrição");

                    return BadRequest(CRMMessages.PostMissingItems("Nivel de Acesso", props));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Nivel de Acesso", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<NivelAcessoModel>> UpdateNivelAcesso(NivelAcessoModel nvAcesso,
            [FromQuery] string? descricao,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var bdNivelAcesso = await _dbcontext.bdNivelAcesso.FindAsync(nvAcesso.IdNivelAcesso);
                if (bdNivelAcesso == null)
                    return BadRequest(CRMMessages.NotFind("Nivel de Acesso"));
                else
                {
                    bdNivelAcesso.DescricaoNivel = descricao ?? bdNivelAcesso.DescricaoNivel;
                    bdNivelAcesso.Ativo = ativo ?? bdNivelAcesso.Ativo;
                    bdNivelAcesso.DataDesativado = datadesativado ?? bdNivelAcesso.DataDesativado;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(bdNivelAcesso);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Nivel de Acesso", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
