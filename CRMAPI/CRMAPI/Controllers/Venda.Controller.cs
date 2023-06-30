using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public VendaController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<VendaModel>>> GetVenda(
            [FromQuery] int[]? ids,
            [FromQuery] int[]? idcliente,
            [FromQuery] int[]? idoperador,
            [FromQuery] int[]? idformapagamento,
            [FromQuery] int[]? idproduto,
            [FromQuery] int[]? idaprovador,
            [FromQuery] int[]? idcriador,
            [FromQuery] int[]? statusvenda,
            [FromQuery] decimal[]? valorvenda,
            [FromQuery] decimal[]? entradavenda,
            [FromQuery] string? cep,
            [FromQuery] string? logradouro,
            [FromQuery] string? numeroendereco,
            [FromQuery] string? bairro,
            [FromQuery] string? complemento,
            [FromQuery] string? estado,
            [FromQuery] string? cidade,
            [FromQuery] string? pais,
            [FromQuery] DateTime? dataassinatura,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? dataassinaturaBetween,
            [FromQuery] DateTime? datacriacaoBetween)
        {
            try
            {
                IQueryable<VendaModel> query = _dbcontext.bdVenda;

                #region Filter
                var hasFilter =
                    ids != null ||
                    idcliente != null ||
                    idoperador != null ||
                    idformapagamento != null ||
                    idproduto != null ||
                    idaprovador != null ||
                    idcriador != null ||
                    statusvenda != null ||
                    valorvenda != null ||
                    entradavenda != null ||
                    cep != null ||
                    logradouro != null ||
                    numeroendereco != null ||
                    bairro != null ||
                    complemento != null ||
                    estado != null ||
                    cidade != null ||
                    pais != null ||
                    dataassinatura != null ||
                    datacriacao != null ||
                    dataassinaturaBetween != null ||
                    datacriacaoBetween != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdVenda
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdVenda
                        .Where(x => ids.Contains(x.IdVenda))
                        .ToListAsync();

                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || dataassinatura != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    VendaModel model = new VendaModel
                    {
                        DataCriacao = (DateTime)datacriacao
                    };

                    if (datacriacao != null || datacriacaoBetween != null)
                    {
                        startDate = datacriacao.HasValue ? datacriacao.Value.Date : null;
                        endDate = datacriacaoBetween.HasValue ? datacriacaoBetween.Value.Date : null;
                    }
                    else if (dataassinatura != null || dataassinaturaBetween != null)
                    {
                        startDate = dataassinatura.HasValue ? dataassinatura.Value.Date : null;
                        endDate = dataassinaturaBetween.HasValue ? dataassinaturaBetween.Value.Date : null;
                    }

                    var dataFilter = datacriacao != null || datacriacaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<VendaModel>(startDate, endDate, "DataCriacao")
                    : dataassinatura != null || dataassinaturaBetween != null
                    ? EndPointDataFilter.CreateDataFilter<VendaModel>(startDate, endDate, "DataAssinatura") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                //cep
                if (!string.IsNullOrEmpty(cep))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<VendaModel>("CEP", cep);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //logradouro
                if (!string.IsNullOrEmpty(logradouro))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<VendaModel>("Logradouro", logradouro);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //numeroendereco
                if (!string.IsNullOrEmpty(numeroendereco))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<VendaModel>("NumeroEndereco", numeroendereco);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //bairro
                if (!string.IsNullOrEmpty(bairro))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<VendaModel>("Bairro", bairro);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //complemento
                if (!string.IsNullOrEmpty(complemento))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<VendaModel>("Complemento", complemento);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //estado
                if (!string.IsNullOrEmpty(estado))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<VendaModel>("Estado", estado);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //cidade
                if (!string.IsNullOrEmpty(cidade))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<VendaModel>("Cidade", cidade);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //pais
                if (!string.IsNullOrEmpty(pais))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<VendaModel>("Pais", pais);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Double
                if (valorvenda != null && valorvenda.Length > 0)
                {
                    var decimalFilter = EndPointDecimalFilter.CreateDoublePropertyFilter<VendaModel>("ValorVenda", valorvenda);
                    if (decimalFilter != null)
                        query = query.Where(decimalFilter).AsQueryable();
                }

                if (entradavenda != null && entradavenda.Length > 0)
                {
                    var decimalFilter = EndPointDecimalFilter.CreateDoublePropertyFilter<VendaModel>("EntradaVenda", entradavenda);
                    if (decimalFilter != null)
                        query = query.Where(decimalFilter).AsQueryable();
                }
                #endregion

                #region Ids
                //IdCriador
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<VendaModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                //IdCliente
                if (idcliente != null && idcliente.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<VendaModel>("IdCliente", idcliente);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                //StatusVenda
                if (statusvenda != null && statusvenda.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<VendaModel>("StatusVenda", statusvenda);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                //IdAprovador
                if (idaprovador != null && idaprovador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<VendaModel>("IdAprovador", idaprovador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                //IdOperador
                if (idoperador != null && idoperador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<VendaModel>("IdOperador", idoperador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                //IdFormaPagamento
                if (idformapagamento != null && idformapagamento.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<VendaModel>("IdFormaPagamento", idformapagamento);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                //IdProduto
                if (idproduto != null && idproduto.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<VendaModel>("IdProduto", idproduto);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Venda", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<VendaModel>>> CreateVenda(
            [FromQuery] int idcliente = 0,
            [FromQuery] int idoperador = 0,
            [FromQuery] int idformapagamento = 0,
            [FromQuery] int idproduto = 0,
            [FromQuery] int? idaprovador = 0,
            [FromQuery] int idcriador = 0,
            [FromQuery] int? statusvenda = 0,
            [FromQuery] decimal? valorvenda = 0,
            [FromQuery] decimal? entradavenda = 0,
            [FromQuery] string cep = "",
            [FromQuery] string? logradouro = "",
            [FromQuery] string? numeroendereco = "",
            [FromQuery] string? bairro = "",
            [FromQuery] string? complemento = "",
            [FromQuery] string? estado = "",
            [FromQuery] string? cidade = "",
            [FromQuery] string pais = "",
            [FromQuery] DateTime dataassinatura = default,
            [FromQuery] DateTime datacriacao = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(cep) &&
                    !string.IsNullOrEmpty(pais) &&
                    idcliente != 0 &&
                    idoperador != 0 &&
                    idformapagamento != 0 &&
                    idproduto != 0)
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Now;

                    if (dataassinatura == default)
                        dataassinatura = DateTime.Now;

                    var newVenda = new VendaModel
                    {
                        ValorVenda = valorvenda != 0 ? null : valorvenda,
                        EntradaVenda = entradavenda != 0 ? null : entradavenda,
                        StatusVenda = statusvenda != 0 ? null : statusvenda,
                        CEP = cep,
                        Logradouro = !string.IsNullOrEmpty(logradouro) ? null : logradouro,
                        NumeroEndereco = !string.IsNullOrEmpty(numeroendereco) ? null : numeroendereco,
                        Bairro = !string.IsNullOrEmpty(bairro) ? null : bairro,
                        Complemento = !string.IsNullOrEmpty(complemento) ? null : complemento,
                        Estado = !string.IsNullOrEmpty(estado) ? null : estado,
                        Cidade = !string.IsNullOrEmpty(cidade) ? null : cidade,
                        Pais = pais,
                        IdAprovador = idaprovador,
                        IdCliente = idcliente,
                        IdOperador = idoperador,
                        IdFormaPagamento = idformapagamento,
                        IdProduto = idproduto,
                        DataAssinatura = dataassinatura,
                        DataCriacao = datacriacao,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdVenda.Add(newVenda);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdVenda.ToListAsync());
                }
                else
                {
                    string? props;
                    props = !string.IsNullOrEmpty(cep) ? null : Concat.ConcatString("CEP");
                    props = !string.IsNullOrEmpty(pais) ? null : Concat.ConcatString("Pais");
                    props = idcliente != 0 ? null : Concat.ConcatString("Cliente");
                    props = idoperador != 0 ? null : Concat.ConcatString("Operador da Venda");
                    props = idformapagamento != 0 ? null : Concat.ConcatString("Forma de Pagamento");
                    props = idproduto != 0 ? null : Concat.ConcatString("Produto");

                    return BadRequest(CRMMessages.PostMissingItems("Venda", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Venda", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<VendaModel>> UpdateVenda(VendaModel vendaModel,
            [FromQuery] int? idformapagamento,
            [FromQuery] int? idaprovador,
            [FromQuery] int? statusvenda,
            [FromQuery] decimal? valorvenda,
            [FromQuery] decimal? entradavenda,
            [FromQuery] string? cep,
            [FromQuery] string? logradouro,
            [FromQuery] string? numeroendereco,
            [FromQuery] string? bairro,
            [FromQuery] string? complemento,
            [FromQuery] string? estado,
            [FromQuery] string? cidade,
            [FromQuery] string? pais,
            [FromQuery] DateTime? dataassinatura)
        {
            try
            {
                var dbVenda = await _dbcontext.bdVenda.FindAsync();
                if (dbVenda == null)
                    return BadRequest(CRMMessages.NotFind("Venda"));
                else
                {
                    dbVenda.IdAprovador = idaprovador ?? dbVenda.IdAprovador;
                    dbVenda.StatusVenda = statusvenda ?? dbVenda.StatusVenda;
                    dbVenda.IdFormaPagamento = idformapagamento ?? dbVenda.IdFormaPagamento;
                    dbVenda.ValorVenda = valorvenda ?? dbVenda.ValorVenda;
                    dbVenda.EntradaVenda = entradavenda ?? dbVenda.EntradaVenda;
                    dbVenda.CEP = cep ?? dbVenda.CEP;
                    dbVenda.Logradouro = logradouro ?? dbVenda.Logradouro;
                    dbVenda.NumeroEndereco = numeroendereco ?? dbVenda.NumeroEndereco;
                    dbVenda.Bairro = bairro ?? dbVenda.Bairro;
                    dbVenda.Complemento = complemento ?? dbVenda.Complemento;
                    dbVenda.Estado = estado ?? dbVenda.Estado;
                    dbVenda.Cidade = cidade ?? dbVenda.Cidade;
                    dbVenda.Pais = pais ?? dbVenda.Pais;
                    dbVenda.DataAssinatura = dataassinatura ?? dbVenda.DataAssinatura;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(dbVenda);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Venda", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
