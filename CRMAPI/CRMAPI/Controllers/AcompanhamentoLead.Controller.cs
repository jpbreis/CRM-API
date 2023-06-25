using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcompanhamentoLeadController : ControllerBase
    {
        private readonly DataContext _dbcontext;
        readonly string idsProperty = "IdAcompanhamento";
        readonly string descricaoProperty = "DescricaoAcompanhamento";
        readonly string datacriacaoProperty = "DataCriacao";
        readonly string datadesativadoProperty = "DataDesativado";
        readonly string ativoProperty = "Ativo";
        readonly string idcriadorProperty = "IdCriador";

        public AcompanhamentoLeadController(DataContext contex)
        {
            _dbcontext = contex;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<AcompanhamentoLeadModel>>> GetAcompanhamentoLead(
            [FromQuery] int[]? ids,
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

                IQueryable<AcompanhamentoLeadModel> query = _dbcontext.bdAcompanhamentoLead;

                #region (Otimizador em Teste) Foreach
                //foreach (var parameter in new object[] { IdAcompanhamento, descricaoacompanhamento, ativo, idcriador, datacriacao, datadesativado, datacriacaoBetween, datadesativadoBetween })
                //{
                //    var parameterType = parameter.GetType();
                //    var propertyName = GetProperty.GetPropertyName<AcompanhamentoLeadModel>((string)parameter);
                //
                //    if (parameterType == typeof(string))
                //    {
                //        var stringFilter = EndPointFilter.PassProp<AcompanhamentoLeadModel>(stringName: propertyName, stringValue: (string)parameter);
                //
                //        if (stringFilter != null)
                //            query = query.Where(stringFilter).AsQueryable();
                //    }
                //    else if (parameterType == typeof(int))
                //    {
                //        var intFilter = EndPointFilter.PassProp<AcompanhamentoLeadModel>(stringName: propertyName, intValue: (int[])parameter);
                //
                //        if (intFilter != null)
                //            query = query.Where(intFilter).AsQueryable();
                //    }
                //    else if (parameterType == typeof(bool))
                //    {
                //        var parameterValue = (bool)parameter;
                //
                //        if (!string.IsNullOrEmpty(propertyName))
                //        {
                //            var parameterExpression = Expression.Parameter(typeof(AcompanhamentoLeadModel), "o");
                //            var propertyExpression = Expression.Property(parameterExpression, propertyName);
                //            var equalExpression = Expression.Equal(propertyExpression, Expression.Constant(parameterValue));
                //
                //            var boolFilter = Expression.Lambda<Func<AcompanhamentoLeadModel, bool>>(equalExpression, parameterExpression);
                //
                //            query = query.Where(boolFilter).AsQueryable();
                //        }
                //    }
                //    else
                //        return Ok(await query.ToListAsync());
                //}
                //
                //return Ok(await query.ToListAsync());
                #endregion

                #region Filter
                bool hasFilter =
                    ids != null ||
                    descricao != null ||
                    ativo != null ||
                    idcriador != null ||
                    datacriacao != null ||
                    datadesativado != null ||
                    datacriacaoBetween != null ||
                    datacriacaoBetween != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdAcompanhamento
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdAcompanhamentoLead
                        .Where(x => ids.Contains(x.IdAcompanhamento))
                        .ToListAsync();
                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    AcompanhamentoLeadModel model = new AcompanhamentoLeadModel
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
                    ? EndPointDataFilter.CreateDataFilter<AcompanhamentoLeadModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<AcompanhamentoLeadModel>(startDate, endDate, "DataDesativado")
                    : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(descricao))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<AcompanhamentoLeadModel>("DescricaoAcompanhamento", descricao);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsCriadorFilter = EndPointIntFilter.CreateIntPropertyFilter<AcompanhamentoLeadModel>("IdCriador", idcriador);
                    if (idsCriadorFilter != null)
                        query = query.Where(idsCriadorFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdAcompanhamentoLead;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("AcompanhamentoLead", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<AcompanhamentoLeadModel>>> CreteAcompanhamento(
            [FromQuery] string descricao = "",
            [FromQuery] bool? ativo = true,
            [FromQuery] int idcriador = 0,
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] DateTime? datadesativado = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(descricao))
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Today;

                    var newAcompanhamentoLead = new AcompanhamentoLeadModel
                    {
                        DescricaoAcompanhamento = descricao,
                        DataCriacao = string.IsNullOrEmpty(datacriacao.ToString()) ? DateTime.Today : datacriacao, 
                        DataDesativado = string.IsNullOrEmpty(datadesativado.ToString()) ? null : datadesativado,
                        Ativo = ativo,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdAcompanhamentoLead.Add(newAcompanhamentoLead);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdAcompanhamentoLead.ToListAsync());
                }
                else
                {
                    string props = !string.IsNullOrEmpty(descricao) ? null : Concat.ConcatString("Descrição");

                    return BadRequest(CRMMessages.PostMissingItems("AcompanhamentoLead", props));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("AcompanhamentoLead", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<AcompanhamentoLeadModel>> UpadateAcompanhamento(AcompanhamentoLeadModel acLead,
            [FromQuery] string? descricao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] bool? ativo)
        {
            try
            {
                var bdAcompLead = await _dbcontext.bdAcompanhamentoLead.FindAsync(acLead.IdAcompanhamento);
                if (bdAcompLead == null)
                    return BadRequest(CRMMessages.NotFind("Acompanhamentos de Lead"));
                else
                {
                    bdAcompLead.DescricaoAcompanhamento = descricao ?? bdAcompLead.DescricaoAcompanhamento;
                    bdAcompLead.DataDesativado = datadesativado ?? bdAcompLead.DataDesativado;
                    bdAcompLead.Ativo = ativo ?? bdAcompLead.Ativo;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(bdAcompLead);
                }   
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError($"AcompanhementoLead: {acLead.IdAcompanhamento} - {acLead.DescricaoAcompanhamento}", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        [HttpDelete("{IdAcompanhamentoLead}")]
        public async Task<ActionResult<List<AcompanhamentoLeadModel>>> DeleteLead(int idAcLead)
        {
            try
            {
                var bdAcLead = await _dbcontext.bdAcompanhamentoLead.FindAsync(idAcLead);
                if (bdAcLead == null)
                    return BadRequest(CRMMessages.NotFind("Acompanhamentos de Lead"));
                else
                {
                    _dbcontext.bdAcompanhamentoLead.Remove(bdAcLead);

                    await _dbcontext.SaveChangesAsync();
                    return Ok(CRMMessages.DeleteOk($"{bdAcLead.IdAcompanhamento} - {bdAcLead.DescricaoAcompanhamento}", "Acompanhamentos de Lead"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.DeleteError("Acompanhamentos De Lead", ex.ToString()));
            }
        }
        #endregion
    }
}
