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
    public class FormaPagamentoController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public FormaPagamentoController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<FormaPagamentoModel>>> GetFormaPagamento(
            [FromQuery] int[]? ids,
            [FromQuery] string? formapagamento,
            [FromQuery] string? descricao,
            [FromQuery] bool? ativo,
            [FromQuery] int[]? idcriador,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? dataalteracao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? dataalteracaoBetween,
            [FromQuery] DateTime? datadesativadoBetween)
        {
            try
            {
                IQueryable<FormaPagamentoModel> query = _dbcontext.bdFormaPagamento;

                #region Filter
                var hasFilter =
                    ids != null ||
                    !string.IsNullOrEmpty(formapagamento) ||
                    !string.IsNullOrEmpty(descricao) ||
                    ativo != null ||
                    idcriador != null ||
                    datacriacao != null ||
                    dataalteracao != null ||
                    datadesativado != null ||
                    datacriacaoBetween != null ||
                    dataalteracaoBetween != null ||
                    datadesativadoBetween != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdFormaPagamento
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdFormaPagamento
                        .Where(x => ids.Contains(x.IdFormaPagamento))
                        .ToListAsync();

                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || dataalteracao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    FormaPagamentoModel model = new FormaPagamentoModel
                    {
                        DataCriacao = (DateTime)datacriacao,
                    };

                    if (datacriacao != null || datacriacaoBetween != null)
                    {
                        startDate = datacriacao.HasValue ? datacriacao.Value.Date : null;
                        endDate = datacriacaoBetween.HasValue ? datacriacaoBetween.Value.Date : null;
                    }
                    else if (dataalteracao != null || dataalteracaoBetween != null)
                    {
                        startDate = dataalteracao.HasValue ? dataalteracao.Value.Date : null;
                        endDate = dataalteracaoBetween.HasValue ? dataalteracaoBetween.Value.Date : null;
                    }
                    else if (datadesativado != null || datadesativadoBetween != null)
                    {
                        startDate = datadesativado.HasValue ? datadesativado.Value.Date : null;
                        endDate = datadesativadoBetween.HasValue ? datadesativadoBetween.Value.Date : null;
                    }

                    var dataFilter = datacriacao != null || datacriacaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<FormaPagamentoModel>(startDate, endDate, "DataCriacao")
                    : dataalteracao != null || dataalteracaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<FormaPagamentoModel>(startDate, endDate, "DataAlteracao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<FormaPagamentoModel>(startDate, endDate, "DataDesativado")
                    : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(formapagamento))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<FormaPagamentoModel>("FormaPagamento", formapagamento);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                if (!string.IsNullOrEmpty(descricao))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<FormaPagamentoModel>("DescricaoFormaPagamento", descricao);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<FormaPagamentoModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdFormaPagamento;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Forma de Pagamento", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<FormaPagamentoModel>>> CreateFormaPagamento(
            [FromQuery] string formapagamento = "",
            [FromQuery] string descricao = "",
            [FromQuery] bool ativo = true,
            [FromQuery] int idcriador = 0,
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] DateTime? dataalteracao = null,
            [FromQuery] DateTime? datadesativado = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(formapagamento) && !string.IsNullOrEmpty(descricao))
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Today;

                    var newFormaPagamento = new FormaPagamentoModel
                    {
                        FormaPagamento = formapagamento,
                        DescricaoFormaPagamento = descricao,
                        DataCriacao = datacriacao,
                        DataAlteracao = !string.IsNullOrEmpty(dataalteracao.ToString()) ? null : dataalteracao,
                        DataDesativado = !string.IsNullOrEmpty(datadesativado.ToString()) ? null : datadesativado,
                        Ativo = ativo,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdFormaPagamento.Add(newFormaPagamento);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdFormaPagamento.ToListAsync());
                }
                else
                {
                    string? props;
                    props = !string.IsNullOrEmpty(formapagamento) ? null : Concat.ConcatString("Forma de Pagamento");
                    props = !string.IsNullOrEmpty(descricao) ? null : Concat.ConcatString("Descrição");

                    return BadRequest(CRMMessages.PostMissingItems("Forma de Pagamento", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Forma de Pagamento", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<FormaPagamentoModel>> UpdateFormaPagamento(FormaPagamentoModel formaPagamentoModel,
            [FromQuery] string? formapagamento,
            [FromQuery] string? descricao,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? dataalteracao,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var dbFormaPagamento = await _dbcontext.bdFormaPagamento.FindAsync(formaPagamentoModel.IdFormaPagamento);
                if (dbFormaPagamento == null)
                    return BadRequest(CRMMessages.NotFind("Forma de Pagamento"));
                else
                {
                    dbFormaPagamento.FormaPagamento = formapagamento ?? dbFormaPagamento.FormaPagamento;
                    dbFormaPagamento.DescricaoFormaPagamento = descricao ?? dbFormaPagamento.DescricaoFormaPagamento;
                    dbFormaPagamento.DataAlteracao = dataalteracao ?? dbFormaPagamento.DataAlteracao;
                    dbFormaPagamento.DataDesativado = datadesativado ?? dbFormaPagamento.DataDesativado;
                    dbFormaPagamento.Ativo = ativo ?? dbFormaPagamento.Ativo;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(dbFormaPagamento);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Forma de Pagamento", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
