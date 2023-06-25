using CRMAPI.Data;
using CRMAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System;
using CRMAPI.Utilities;
using System.Linq;
using Microsoft.Identity.Client;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdicionaisProdutoController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public AdicionaisProdutoController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<AdicionaisProdutoModel>>> GetAdicionaisProduto(
            [FromQuery] int[]? ids,
            [FromQuery] int[]? idproduto,
            [FromQuery] string? sigla,
            [FromQuery] string? descricao,
            //[FromQuery] valor,
            [FromQuery] bool? ativo,
            [FromQuery] int[]? idcriador,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? datadesativadoBetween)
        {
            try
            {
                IQueryable<AdicionaisProdutoModel> query = _dbcontext.bdAdicionaisProduto;

                #region Filter
                var hasFilter =
                    ids != null ||
                    idproduto != null ||
                    !string.IsNullOrEmpty(sigla) ||
                    !string.IsNullOrEmpty(descricao) ||
                    //valor != null ||
                    datacriacao != null ||
                    datadesativado != null ||
                    datacriacaoBetween != null ||
                    datadesativadoBetween != null ||
                    ativo != null ||
                    idcriador != null;

                if (!hasFilter)
                    return Ok(await _dbcontext.bdAdicionaisProduto.ToListAsync());
                #endregion

                #region IdAdicionalProduto
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdAdicionaisProduto
                        .Where(x => ids.Contains(x.IdAdicionalProduto))
                        .ToListAsync();

                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    AdicionaisProdutoModel model = new AdicionaisProdutoModel
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
                    ? EndPointDataFilter.CreateDataFilter<AdicionaisProdutoModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<AdicionaisProdutoModel>(startDate, endDate, "DataDesativado") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(sigla))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<AdicionaisProdutoModel>("SiglaAdicionalProduto", sigla);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                if (!string.IsNullOrEmpty(descricao))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<AdicionaisProdutoModel>("DescricaoAdicionalProduto", descricao);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<AdicionaisProdutoModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                if (idproduto != null && idproduto.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<AdicionaisProdutoModel>("IdProduto", idproduto);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdAdicionaisProduto;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Adicionais Proutos", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<AdicionaisProdutoModel>>> CreateAdicionalProduto(
            [FromQuery] int idproduto = 0,
            [FromQuery] string sigla = "",
            [FromQuery] string descricao = "",
            [FromQuery] double? valor = 0,
            [FromQuery] bool ativo = true,
            [FromQuery] int idcriador = 0,
            [FromQuery] DateTime datacriacao = default, 
            [FromQuery] DateTime? datadesativado = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(sigla) && !string.IsNullOrEmpty(descricao))
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Today;

                    var newAdicionalProduto = new AdicionaisProdutoModel
                    {
                        IdProduto = idproduto,
                        SiglaAdicionalProduto = sigla,
                        DescricaoAdicionalProduto = descricao,
                        ValorAdicionalProduto = (valor != 0 ? null : valor),
                        DataCriacao = datacriacao,
                        DataDesativado =!string.IsNullOrEmpty(datadesativado.ToString()) ? null : datadesativado,
                        Ativo = ativo,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdAdicionaisProduto.Add(newAdicionalProduto);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdAdicionaisProduto.ToListAsync());
                }
                else
                {
                    string? props;
                    props = !string.IsNullOrEmpty(sigla) ? null : Concat.ConcatString("Sigla do Adicional");
                    props = !string.IsNullOrEmpty(descricao) ? null : Concat.ConcatString("Descrição");

                    return BadRequest(CRMMessages.PostMissingItems("Adicional dos Produto", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Adicionais Produto", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<AdicionaisProdutoModel>> UpdateAdicionaisProcesso(AdicionaisProdutoModel adicionaisProdutoModel,
            [FromQuery] int? idproduto,
            [FromQuery] string? sigla,
            [FromQuery] string? descricao,
            [FromQuery] double? valor,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var dbAdicionaisProduto = await _dbcontext.bdAdicionaisProduto.FindAsync(adicionaisProdutoModel.IdAdicionalProduto);
                if (dbAdicionaisProduto == null)
                    return BadRequest(CRMMessages.NotFind("Adicionais Produto"));
                else
                {
                    dbAdicionaisProduto.IdProduto = idproduto ?? dbAdicionaisProduto.IdProduto;
                    dbAdicionaisProduto.SiglaAdicionalProduto = sigla ?? dbAdicionaisProduto.SiglaAdicionalProduto;
                    dbAdicionaisProduto.DescricaoAdicionalProduto = descricao ?? dbAdicionaisProduto.DescricaoAdicionalProduto;
                    dbAdicionaisProduto.ValorAdicionalProduto = valor ?? dbAdicionaisProduto.ValorAdicionalProduto;
                    dbAdicionaisProduto.DataDesativado = datadesativado ?? dbAdicionaisProduto.DataDesativado;
                    dbAdicionaisProduto.Ativo = ativo ?? dbAdicionaisProduto.Ativo;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(dbAdicionaisProduto);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Adicionais Produto", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
