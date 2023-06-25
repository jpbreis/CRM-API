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
    public class ContratoTrabalhoController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public ContratoTrabalhoController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<ContratoTrabalhoModel>>> GetContratoTrabalho(
            [FromQuery] int[]? ids,
            [FromQuery] string? tipo,
            [FromQuery] string? descricao,
            [FromQuery] bool? ativo,
            [FromQuery] int[]? idcriador,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? datadesativadoBetween)
        {
            try
            {
                IQueryable<ContratoTrabalhoModel> query = _dbcontext.bdContratoTrabalho;

                #region Filter
                var hasFilter =
                    ids != null ||
                    !string.IsNullOrEmpty(tipo) ||
                    !string.IsNullOrEmpty(descricao) ||
                    datacriacao != null ||
                    datadesativado != null ||
                    ativo != null ||
                    idcriador != null||
                    datacriacaoBetween != null ||
                    datadesativadoBetween != null;

                if (!hasFilter)
                    return Ok(await _dbcontext.bdContratoTrabalho.ToListAsync());
                #endregion

                #region IdContratoTrabalho
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdContratoTrabalho
                        .Where(x => ids.Contains(x.IdContratoTrabalho))
                        .ToListAsync();

                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    ContratoTrabalhoModel model = new ContratoTrabalhoModel
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
                    ? EndPointDataFilter.CreateDataFilter<ContratoTrabalhoModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<ContratoTrabalhoModel>(startDate, endDate, "DataDesativado") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(tipo))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ContratoTrabalhoModel>("TipoContrato", tipo);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                if (!string.IsNullOrEmpty(descricao))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ContratoTrabalhoModel>("DescricaoContrato", descricao);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<ContratoTrabalhoModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdContratoTrabalho;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Contrato de Trabalho", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<ContratoTrabalhoModel>>> CreateContratoTrabalho(
            [FromQuery] string tipo = "",
            [FromQuery] string descricao = "",
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] bool ativo = true,
            [FromQuery] int idcriador = 0)
        {
            try
            {
                if (tipo != "" && descricao != "")
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Today;

                    var newContratoTrabalho = new ContratoTrabalhoModel
                    {
                        TipoContrato = tipo,
                        DescricaoContrato = descricao,
                        DataCriacao = datacriacao,
                        Ativo = ativo,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdContratoTrabalho.Add(newContratoTrabalho);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdContratoTrabalho.ToListAsync());
                }
                else
                {
                    string? props;
                    props = !string.IsNullOrEmpty(tipo) ? null : Concat.ConcatString("Tipo de Contrato");
                    props = !string.IsNullOrEmpty(descricao) ? null : Concat.ConcatString("Descrição do Contrato");

                    return BadRequest(CRMMessages.PostMissingItems("Contrato de Trabalho", props));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Contrato de Trabalho", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<ContratoTrabalhoModel>> UpdateContratoTrabalho(ContratoTrabalhoModel contTrab,
            [FromQuery] string? tipo,
            [FromQuery] string? descricao,
            [FromQuery] bool? ativo,
            [FromQuery] int? idcriador,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var dbContTrab = await _dbcontext.bdContratoTrabalho.FindAsync(contTrab.IdContratoTrabalho);
                if (dbContTrab == null)
                    return BadRequest(CRMMessages.NotFind("Contrato de Trabalho"));
                else
                {
                    dbContTrab.TipoContrato = tipo ?? dbContTrab.TipoContrato;
                    dbContTrab.DescricaoContrato = descricao ?? dbContTrab.DescricaoContrato;
                    dbContTrab.DataCriacao = datacriacao ?? dbContTrab.DataCriacao;
                    dbContTrab.DataDesativado = datadesativado ?? dbContTrab.DataDesativado;
                    dbContTrab.Ativo = ativo ?? dbContTrab.Ativo;
                    dbContTrab.IdCriador = idcriador ?? dbContTrab.IdCriador;

                    await _dbcontext.SaveChangesAsync();
                    return Ok(dbContTrab);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Contrato de Trabalho", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
