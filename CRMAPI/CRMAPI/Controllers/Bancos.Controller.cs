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
    public class BancosController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public BancosController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<BancosModel>>> GetBancos(
            [FromQuery] int[]? ids,
            [FromQuery] int[]? codbacen,
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
                IQueryable<BancosModel> query = _dbcontext.bdBancos;

                #region Filter
                var hasFilter =
                    ids != null ||
                    codbacen != null ||
                    nome != null ||
                    ativo != null ||
                    idcriador != null ||
                    datacriacao != null ||
                    datadesativado != null ||
                    datacriacaoBetween != null ||
                    datadesativado != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdBanco
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdBancos
                        .Where(x => ids.Contains(x.IdBanco))
                        .ToListAsync();

                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    BancosModel model = new BancosModel
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
                    ? EndPointDataFilter.CreateDataFilter<BancosModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<BancosModel>(startDate, endDate, "DataDesativado") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(nome))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<BancosModel>("NomeBanco", nome);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<BancosModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdBancos;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Bancos", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<BancosModel>>> CreateBanco(
            [FromQuery] int codbacen = 0,
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

                    var newBanco = new BancosModel
                    {
                        CodBacen = codbacen,
                        NomeBanco = nome,
                        DataCriacao = datacriacao,
                        DataDesativado = string.IsNullOrEmpty(datadesativado.ToString()) ? null : datadesativado,
                        Ativo = ativo,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdBancos.Add(newBanco);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdBancos.ToListAsync());
                }
                else
                {
                    string props = !string.IsNullOrEmpty(nome) ? null : Concat.ConcatString("Nome");

                    return BadRequest(CRMMessages.PostMissingItems("Bancos", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Bancos", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<BancosModel>> UpdateBancos(BancosModel bancoModel,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var dbBanco = await _dbcontext.bdBancos.FindAsync(bancoModel.IdBanco);
                if (dbBanco == null)
                    return BadRequest(CRMMessages.NotFind("Bancos"));
                else
                {
                    dbBanco.DataDesativado = datadesativado ?? dbBanco.DataDesativado;
                    dbBanco.Ativo = ativo ?? dbBanco.Ativo;

                    await _dbcontext.SaveChangesAsync();
                    return Ok(dbBanco);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Bancos", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
