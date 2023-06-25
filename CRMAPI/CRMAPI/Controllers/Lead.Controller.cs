using CRMAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Runtime.CompilerServices;
using System.Linq;
using CRMAPI.Controllers;
using CRMAPI.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System;
using CRMAPI.Models;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public LeadController(DataContext context)
        {
            _dbcontext = context;
        }
          
        #region HttpGet

        #region All
        [HttpGet]
        public async Task<ActionResult<List<LeadModel>>> GetLead(
            [FromQuery] int[]? ids,
            [FromQuery] string? nome,
            [FromQuery] string? telefone,
            [FromQuery] string? celular,
            [FromQuery] string? email,
            [FromQuery] int[]? idsoperador,
            [FromQuery] int[]? idsempresa,
            [FromQuery] int[]? idsgrupoempresa,
            [FromQuery] int[]? idscliente,
            [FromQuery] int[]? idsacompanhamento,
            [FromQuery] int[]? idsmidia,
            [FromQuery] string? outromidia,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? dataalteracao,
            [FromQuery] DateTime? datainteracao,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? dataalteracaoBetween,
            [FromQuery] DateTime? datainteracaoBetween,
            [FromQuery] int[]? idscriacao)
        {
            try
            {
                IQueryable<LeadModel> query = _dbcontext.bdLead;

                #region Filter
                bool hasFilters =
                ids != null ||
                !string.IsNullOrEmpty(nome) ||
                !string.IsNullOrEmpty(telefone) ||
                !string.IsNullOrEmpty(celular) ||
                !string.IsNullOrEmpty(email) ||
                idsoperador != null ||
                idsempresa != null ||
                idsgrupoempresa != null ||
                idscliente != null ||
                idsacompanhamento != null ||
                idsmidia != null ||
                !string.IsNullOrEmpty(outromidia) ||
                datacriacao != null ||
                dataalteracao != null ||
                datainteracao != null ||
                datacriacaoBetween != null ||
                dataalteracaoBetween != null ||
                datainteracaoBetween != null ||
                idscriacao != null;

                if (!hasFilters)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdLead
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdLead
                        .Where(x => ids.Contains(x.IdLead))
                        .ToListAsync();
                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || dataalteracao != null || datainteracao != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    LeadModel model = new LeadModel
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
                    else if (datainteracao != null || datainteracaoBetween != null)
                    {
                        startDate = datainteracao.HasValue ? datainteracao.Value.Date : null;
                        endDate = datainteracaoBetween.HasValue ? datainteracaoBetween.Value.Date : null;
                    }

                    var dataFilter = datacriacao != null || datacriacaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<LeadModel>(startDate, endDate, "DataCriacao")
                    : dataalteracao != null || dataalteracaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<LeadModel>(startDate, endDate, "DataAlteracao")
                    : datainteracao != null || datainteracaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<LeadModel>(startDate, endDate, "DataInteracao")
                    : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(nome))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<LeadModel>("NomeLead", nome);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                if (!string.IsNullOrEmpty(telefone))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<LeadModel>("TelefoneLead", telefone);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                if (!string.IsNullOrEmpty(celular))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<LeadModel>("CelularLead", celular);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                if (!string.IsNullOrEmpty(email))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<LeadModel>("EmailLead", email);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                if (!string.IsNullOrEmpty(outromidia))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<LeadModel>("OutroMidia", outromidia);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idsoperador != null && idsoperador.Length > 0)
                {
                    var idsOperadorFilter = EndPointIntFilter.CreateIntPropertyFilter<LeadModel>("IdOperador", idsoperador);
                    if (idsOperadorFilter != null)
                        query = query.Where(idsOperadorFilter).AsQueryable();
                }

                if (idsempresa != null && idsempresa.Length > 0)
                {
                    var idsEmpresaFilter = EndPointIntFilter.CreateIntPropertyFilter<LeadModel>("IdEmpresa", idsempresa);
                    if (idsEmpresaFilter != null)
                        query = query.Where(idsEmpresaFilter).AsQueryable();
                }

                if (idsgrupoempresa != null && idsgrupoempresa.Length > 0)
                {
                    var idsGrupoEmpresaFilter = EndPointIntFilter.CreateIntPropertyFilter<LeadModel>("IdGrupoEmpresa", idsgrupoempresa);
                    if (idsGrupoEmpresaFilter != null)
                        query = query.Where(idsGrupoEmpresaFilter).AsQueryable();
                }

                if (idscliente != null && idscliente.Length > 0)
                {
                    var idsClienteFilter = EndPointIntFilter.CreateIntPropertyFilter<LeadModel>("IdCliente", idscliente);
                    if (idsClienteFilter != null)
                        query = query.Where(idsClienteFilter).AsQueryable();
                }

                if (idsacompanhamento != null && idsacompanhamento.Length > 0)
                {
                    var idsOperadorFilter = EndPointIntFilter.CreateIntPropertyFilter<LeadModel>("IdAcompanhamento", idsacompanhamento);
                    if (idsOperadorFilter != null)
                        query = query.Where(idsOperadorFilter).AsQueryable();
                }

                if (idsmidia != null && idsmidia.Length > 0)
                {
                    var idsMidiaFilter = EndPointIntFilter.CreateIntPropertyFilter<LeadModel>("IdMidia", idsmidia);
                    if (idsMidiaFilter != null)
                        query = query.Where(idsMidiaFilter).AsQueryable();
                }

                if (idscriacao != null && idscriacao.Length > 0)
                {
                    var idsCriacaoFilter = EndPointIntFilter.CreateIntPropertyFilter<LeadModel>("IdCriador", idscriacao);
                    if (idsCriacaoFilter != null)
                        query = query.Where(idsCriacaoFilter).AsQueryable();
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Lead".GetError(ex.ToString()));
            }
        }
        #endregion

        #region Last
        [HttpGet("Last")]
        public async Task<ActionResult<LeadModel>> GetLastLead()
        {
            try
            {
                var lastLead = await _dbcontext.bdLead
                    .OrderByDescending(x => x.DataCriacao).FirstOrDefaultAsync();
                if (lastLead == null)
                    return BadRequest(CRMMessages.NotFind("Lead"));
                else
                    return Ok(lastLead);
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Lead", ex.ToString()));
            }
        }
        #endregion

        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<LeadModel>>> CreateLead(
            [FromQuery] string nome = "",
            [FromQuery] string? telefone = "",
            [FromQuery] string celular = "",
            [FromQuery] string? email = "",
            [FromQuery] int idoperador = 0,
            [FromQuery] int? idempresa = 0,
            [FromQuery] int? idgrupoempresa = 0,
            [FromQuery] int? idcliente = 0,
            [FromQuery] int? idacompanhamento = 0,
            [FromQuery] int idmidia = 0,
            [FromQuery] string? outromidia = "",
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] DateTime? dataalteracao = null,
            [FromQuery] DateTime? datainteracao = null,
            [FromQuery] int idcriacao = 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(nome) &&
                !string.IsNullOrEmpty(celular) &&
                idoperador != 0 &&
                idmidia != 0 &&
                idcriacao != 0)
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Now;

                    var newLead = new LeadModel
                    {
                        NomeLead = nome,
                        TelefoneLead = string.IsNullOrEmpty(telefone) ? null : telefone,
                        CelularLead = celular,
                        EmailLead = string.IsNullOrEmpty(email) ? null : email,
                        IdOperador = idoperador,
                        IdEmpresa = idempresa,
                        IdGrupoEmpresa = idgrupoempresa == 0 ? null : idgrupoempresa,
                        IdCliente = idcliente == 0 ? null : idcliente,
                        IdAcompanhamento = idacompanhamento == 0 ? null : idacompanhamento,
                        IdMidia = idmidia,
                        OutroMidia = string.IsNullOrEmpty(outromidia) ? null : outromidia,
                        DataCriacao = string.IsNullOrEmpty(datacriacao.ToString()) ? DateTime.Now : datacriacao,
                        DataAlteracao = string.IsNullOrEmpty(dataalteracao.ToString()) ? null : dataalteracao,
                        DataInteracao = string.IsNullOrEmpty(datainteracao.ToString()) ? null : datainteracao,
                        IdCriador = idcriacao
                    };

                    _dbcontext.bdLead.Add(newLead);
                    await _dbcontext.SaveChangesAsync();

                    /*
                     COMO RETORNAR SOMENTE O LEAD ADICIONADO DE FORMA DINAMICA ?
                     */
                    var lastItem = GetLastLead();
                    return Ok(lastItem);
                }
                else
                {
                    string? props;
                    props = !string.IsNullOrEmpty(nome) ? null : Concat.ConcatString("Nome");
                    props = !string.IsNullOrEmpty(celular) ? null : Concat.ConcatString("Celular");
                    props = idmidia != 0 ? null : Concat.ConcatString("Midia");
                    props = idcriacao != 0 ? null : Concat.ConcatString("Usuario");

                    return BadRequest(CRMMessages.PostMissingItems("Lead", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lead".PostError(ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<LeadModel>> UpdateLead(LeadModel lead,
            [FromQuery] string? nome,
            [FromQuery] string? telefone,
            [FromQuery] string? celular,
            [FromQuery] string? email,
            [FromQuery] int? idoperador,
            [FromQuery] int? idempresa,
            [FromQuery] int? idgrupoempresa,
            [FromQuery] int? idcliente,
            [FromQuery] int? idacompanhamento,
            [FromQuery] int? idmidia,
            [FromQuery] string? outromidia,
            [FromQuery] DateTime? dataalteracao,
            [FromQuery] DateTime? datainteracao)
        {
            try
            {
                var bdLead = await _dbcontext.bdLead.FindAsync(lead.IdLead);
                if (bdLead == null)
                    return BadRequest("Lead".NotFind());
                else
                {
                    bdLead.NomeLead = nome ?? bdLead.NomeLead;
                    bdLead.TelefoneLead = telefone ?? bdLead.TelefoneLead;
                    bdLead.CelularLead = celular ?? bdLead.CelularLead;
                    bdLead.EmailLead =email ?? bdLead.EmailLead;
                    bdLead.IdOperador = idoperador ?? bdLead.IdOperador;
                    bdLead.IdEmpresa = idempresa ?? bdLead.IdEmpresa;
                    bdLead.IdGrupoEmpresa = idgrupoempresa ?? bdLead.IdGrupoEmpresa;
                    bdLead.IdCliente = idcliente ?? bdLead.IdCliente;
                    bdLead.IdAcompanhamento = idacompanhamento ?? bdLead.IdAcompanhamento;
                    bdLead.IdMidia = idmidia ?? bdLead.IdMidia;
                    bdLead.OutroMidia = outromidia ?? bdLead.OutroMidia;
                    bdLead.DataAlteracao = dataalteracao ?? bdLead.DataAlteracao;
                    bdLead.DataInteracao = datainteracao ?? bdLead.DataInteracao;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(bdLead);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError($"Lead: {lead.IdLead} - {lead.NomeLead}", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        //[HttpDelete("{idLead}")]
        //public async Task<ActionResult<List<Lead>>> DeleteLead(int idLead)
        //{
        //    try
        //    {
        //        var bdLead = await _dbcontext.bdLead.FindAsync(idLead);
        //        if (bdLead == null)
        //            return BadRequest(CRMMessages.NotFind("Lead"));
        //        else
        //        {
        //            _dbcontext.bdLead.Remove(bdLead);

        //            await _dbcontext.SaveChangesAsync();
        //            return Ok("");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("");
        //    }
        //}
        #endregion
    }
}
