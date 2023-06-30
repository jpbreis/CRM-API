using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq;
using System.Security.Principal;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public ProdutoController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<ProdutoModel>>> GetProduto(
            [FromQuery] int[]? ids,
            [FromQuery] string? nome,
            [FromQuery] string? descricao,
            [FromQuery] decimal[]? valorminimo,
            [FromQuery] int[]? idcriador,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? dataalteracao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? dataalteracaoBetween,
            [FromQuery] DateTime? datadesativadoBetween)
        {
            try
            {
                IQueryable<ProdutoModel> query = _dbcontext.bdProduto;

                #region Filter
                var hasFilter =
                    ids != null ||
                    nome != null ||
                    descricao != null ||
                    valorminimo != null ||
                    idcriador != null ||
                    ativo != null ||
                    datacriacao != null ||
                    dataalteracao != null ||
                    datadesativado != null ||
                    datacriacaoBetween != null ||
                    dataalteracaoBetween != null ||
                    datadesativadoBetween != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdProduto
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdProduto
                        .Where(x => ids.Contains(x.IdProduto))
                        .ToListAsync();

                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null || dataalteracao != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    ProdutoModel model = new ProdutoModel
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
                    else if (dataalteracao != null || dataalteracaoBetween != null)
                    {
                        startDate = dataalteracao.HasValue ? dataalteracao.Value.Date : null;
                        endDate = dataalteracaoBetween.HasValue ? dataalteracaoBetween.Value.Date : null;
                    }

                    var dataFilter = datacriacao != null || datacriacaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<ProdutoModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<ProdutoModel>(startDate, endDate, "DataDesativado")
                    : datadesativado != null || dataalteracaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<ProdutoModel>(startDate, endDate, "DataAlteracao") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(nome))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ProdutoModel>("NomeProduto", nome);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                if (!string.IsNullOrEmpty(descricao))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ProdutoModel>("DescricaoProduto", descricao);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<ProdutoModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region Decimal
                if (valorminimo != null && valorminimo.Length > 0)
                {
                    var decimalFilter = EndPointDecimalFilter.CreateDoublePropertyFilter<ProdutoModel>("ValorMinimo", valorminimo);
                    if (decimalFilter != null)
                        query = query.Where(decimalFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdProduto;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Produto", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<ProdutoModel>>> CreateProduto(
            [FromQuery] string nome = "",
            [FromQuery] string descricao = "",
            [FromQuery] decimal? valorminimo = 0,
            [FromQuery] int idcriador = 0,
            [FromQuery] bool ativo = true,
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] DateTime? dataalteracao = null,
            [FromQuery] DateTime? datadesativado = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(descricao))
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Today;

                    var newProduto = new ProdutoModel
                    {
                        DescricaoProduto = descricao,
                        NomeProduto = nome,
                        IdCriador = idcriador,
                        ValorMinimo = valorminimo != 0 ? null : valorminimo,
                        DataCriacao = datacriacao,
                        DataAlteracao = dataalteracao != null ? null : dataalteracao,
                        DataDesativado = datadesativado != null ? null : datadesativado,
                        Ativo = ativo
                    };

                    _dbcontext.bdProduto.Add(newProduto);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdProduto.ToListAsync());
                }
                else
                {
                    string? props;
                    props = !string.IsNullOrEmpty(nome) ? null : Concat.ConcatString("Nome");
                    props = !string.IsNullOrEmpty(descricao) ? null : Concat.ConcatString("Descrição");

                    return BadRequest(CRMMessages.PostMissingItems("Produto", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Produto", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<ProdutoModel>> UpdateProduto(ProdutoModel produtoModel,
            [FromQuery] string? nome,
            [FromQuery] string? descricao,
            [FromQuery] decimal? valorminimo,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? dataalteracao,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var dbProduto = await _dbcontext.bdProduto.FindAsync(produtoModel.IdProduto);
                if (dbProduto == null)
                    return BadRequest(CRMMessages.NotFind("Produto"));
                else
                {
                    dbProduto.DescricaoProduto = descricao ?? dbProduto.DescricaoProduto;
                    dbProduto.NomeProduto = nome ?? dbProduto.NomeProduto;
                    dbProduto.ValorMinimo = valorminimo ?? dbProduto.ValorMinimo;
                    dbProduto.DataAlteracao = dataalteracao ?? dbProduto.DataAlteracao;
                    dbProduto.DataDesativado = datadesativado ?? dbProduto.DataDesativado;
                    dbProduto.Ativo = ativo ?? dbProduto.Ativo;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(dbProduto);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Produto", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
