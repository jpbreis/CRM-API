using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MidiaController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public MidiaController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<MidiaModel>>> GetMidia(
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
                IQueryable<MidiaModel> query = _dbcontext.bdMidia;

                #region Filter
                var hasFilter =
                    ids != null ||
                    !string.IsNullOrEmpty(nome) || 
                    datacriacao != null ||
                    datadesativado != null ||
                    ativo != null ||
                    idcriador != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdMidia
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdMidia
                        .Where(x => ids.Contains(x.IdMidia))
                        .ToListAsync();

                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    MidiaModel model = new MidiaModel
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
                    ? EndPointDataFilter.CreateDataFilter<MidiaModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<MidiaModel>(startDate, endDate, "DataDesativado") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(nome))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<MidiaModel>("NomeMidia", nome);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<MidiaModel>("NomeMidia", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdMidia;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Midia", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<MidiaModel>>> CreateMidia(
            [FromQuery] string nome = "",
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] bool ativo = true,
            [FromQuery] int idcriador = 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(nome))
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Today;

                    var newMidia = new MidiaModel
                    {
                        NomeMidia = nome,
                        DataCriacao = datacriacao,
                        Ativo = ativo,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdMidia.Add(newMidia);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdMidia.ToListAsync());
                }
                else
                {
                    string props = !string.IsNullOrEmpty(nome) ? null : Concat.ConcatString("Nome");

                    return BadRequest(CRMMessages.PostMissingItems("Midia", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Midia", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<MidiaModel>> UpdateMidia(MidiaModel midiaModel,
            [FromQuery] string? nome,
            [FromQuery] bool? ativo,
            [FromQuery] int? idcriador,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var bdMidia = await _dbcontext.bdMidia.FindAsync(midiaModel.IdMidia);
                if (bdMidia == null)
                    return BadRequest(CRMMessages.NotFind("Midia"));
                else
                {
                    bdMidia.NomeMidia = nome ?? bdMidia.NomeMidia;
                    bdMidia.DataDesativado = datadesativado ?? bdMidia.DataDesativado;
                    bdMidia.Ativo = ativo ?? bdMidia.Ativo;
                    bdMidia.IdCriador = idcriador ?? bdMidia.IdCriador;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(bdMidia);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Midia", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
