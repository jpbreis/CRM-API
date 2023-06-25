using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public ClienteController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<ClienteModel>>> GetCliente(
            [FromQuery] int[]? ids,
            [FromQuery] string? nome,
            [FromQuery] string? cpf,
            [FromQuery] string? rg,
            [FromQuery] string? cnh,
            [FromQuery] string? celular,
            [FromQuery] string? telefone,
            [FromQuery] string? email,
            [FromQuery] string? cnhcategoria,
            [FromQuery] string? cnhestadoemissor,
            [FromQuery] string? cnhrestricoes,
            [FromQuery] string? cnhrenach,
            [FromQuery] string? cnhcodseguranca,
            [FromQuery] string? cep,
            [FromQuery] string? logradouro,
            [FromQuery] string? numeroendereco,
            [FromQuery] string? bairro,
            [FromQuery] string? complemento,
            [FromQuery] string? estado,
            [FromQuery] string? cidade,
            [FromQuery] string? pais,
            [FromQuery] string? outromidia,
            [FromQuery] bool? blacklist,
            [FromQuery] int[]? idmidia,
            [FromQuery] int[]? idoperador,
            [FromQuery] int[]? idempresa,
            [FromQuery] int[]? idgrupoempresa,
            [FromQuery] int[]? idcriador,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? dataalteracao,
            [FromQuery] DateTime? datanascimento,
            [FromQuery] DateTime? cnhvalidade,
            [FromQuery] DateTime? cnhdataemissao,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? dataalteracaoBetween,
            [FromQuery] DateTime? datanascimentoBetween,
            [FromQuery] DateTime? cnhvalidadeBetween,
            [FromQuery] DateTime? cnhdataemissaoBetween)
        {
            try
            {
                IQueryable<ClienteModel> query = _dbcontext.bdCliente;

                #region Filter
                var hasFilter =
                    ids != null ||
                    !string.IsNullOrEmpty(nome) ||
                    !string.IsNullOrEmpty(cpf) ||
                    !string.IsNullOrEmpty(rg) ||
                    !string.IsNullOrEmpty(cnh) ||
                    !string.IsNullOrEmpty(celular) ||
                    !string.IsNullOrEmpty(telefone) ||
                    !string.IsNullOrEmpty(email) ||
                    !string.IsNullOrEmpty(cnhcategoria) ||
                    !string.IsNullOrEmpty(cnhestadoemissor) ||
                    !string.IsNullOrEmpty(cnhrestricoes) ||
                    !string.IsNullOrEmpty(cnhrenach) ||
                    !string.IsNullOrEmpty(cnhcodseguranca) ||
                    !string.IsNullOrEmpty(cep) ||
                    !string.IsNullOrEmpty(logradouro) ||
                    !string.IsNullOrEmpty(numeroendereco) ||
                    !string.IsNullOrEmpty(bairro) ||
                    !string.IsNullOrEmpty(complemento) ||
                    !string.IsNullOrEmpty(estado) ||
                    !string.IsNullOrEmpty(cidade) ||
                    !string.IsNullOrEmpty(pais) ||
                    !string.IsNullOrEmpty(outromidia) ||
                    blacklist != null ||
                    idmidia != null ||
                    idoperador != null ||
                    idempresa != null ||
                    idgrupoempresa != null ||
                    idcriador != null ||
                    datacriacao != null ||
                    dataalteracao != null ||
                    datanascimento != null ||
                    cnhvalidade != null ||
                    cnhdataemissao != null;

                if (!hasFilter)
                    return Ok(await query.ToArrayAsync());
                #endregion

                #region IdCliente
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdCliente
                        .Where(x => ids.Contains(x.IdCliente))
                        .ToListAsync();

                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || dataalteracao != null || datanascimento != null || cnhvalidade != null || cnhdataemissao != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    DepartamentoModel model = new DepartamentoModel
                    {
                        DataCriacao = (DateTime)datacriacao
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
                    else if (datanascimento != null || datanascimentoBetween != null)
                    {
                        startDate = datanascimento.HasValue ? datanascimento.Value.Date : null;
                        endDate = datanascimentoBetween.HasValue ? datanascimentoBetween.Value.Date : null;
                    }
                    else if (cnhvalidade != null || cnhvalidadeBetween != null)
                    {
                        startDate = cnhvalidade.HasValue ? cnhvalidade.Value.Date : null;
                        endDate = cnhvalidadeBetween.HasValue ? cnhvalidadeBetween.Value.Date : null;
                    }
                    else if (cnhdataemissao != null || cnhdataemissaoBetween != null)
                    {
                        startDate = cnhdataemissao.HasValue ? cnhdataemissao.Value.Date : null;
                        endDate = cnhdataemissaoBetween.HasValue ? cnhdataemissaoBetween.Value.Date : null;
                    }

                    var dataFilter = datacriacao != null || datacriacaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<ClienteModel>(startDate, endDate, "DataCriacao")
                    : dataalteracao != null || dataalteracaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<ClienteModel>(startDate, endDate, "DataAlteracao")
                    : datanascimento != null || datanascimentoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<ClienteModel>(startDate, endDate, "DataNascimentoCliente")
                    : cnhvalidade != null || cnhvalidadeBetween != null
                    ? EndPointDataFilter.CreateDataFilter<ClienteModel>(startDate, endDate, "CNHValidade")
                    : cnhdataemissao != null || cnhdataemissaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<ClienteModel>(startDate, endDate, "CNHDataEmissao") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(nome))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("NomeCliente", nome);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //CPFCliente
                if (!string.IsNullOrEmpty(cpf))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("CPFCliente", cpf);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //RGOCliente
                if (!string.IsNullOrEmpty(rg))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("RGOCliente", rg);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //CNHCliente
                if (!string.IsNullOrEmpty(cnh))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("CNHCliente", cnh);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //Celular
                if (!string.IsNullOrEmpty(celular))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("Celular", celular);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //Telefone
                if (!string.IsNullOrEmpty(telefone))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("Telefone", telefone);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //EmailCliente
                if (!string.IsNullOrEmpty(email))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("EmailCliente", email);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //CNHCategoria
                if (!string.IsNullOrEmpty(cnhcategoria))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("CNHCategoria", cnhcategoria);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //CNHEstadoEmissor
                if (!string.IsNullOrEmpty(cnhestadoemissor))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("CNHEstadoEmissor", cnhestadoemissor);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //CNHRestricoes
                if (!string.IsNullOrEmpty(cnhrestricoes))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("CNHRestricoes", cnhrestricoes);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //CNHRenach
                if (!string.IsNullOrEmpty(cnhrenach))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("CNHRenach", cnhrenach);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //CNHCodSeguranca
                if (!string.IsNullOrEmpty(cnhcodseguranca))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("CNHCodSeguranca", cnhcodseguranca);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //cep
                if (!string.IsNullOrEmpty(cep))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("CEP", cep);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //logradouro
                if (!string.IsNullOrEmpty(logradouro))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("Logradouro", logradouro);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //numeroendereco
                if (!string.IsNullOrEmpty(numeroendereco))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("NumeroEndereco", numeroendereco);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //bairro
                if (!string.IsNullOrEmpty(bairro))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("Bairro", bairro);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //complemento
                if (!string.IsNullOrEmpty(complemento))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("Complemento", complemento);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //estado
                if (!string.IsNullOrEmpty(estado))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("Estado", estado);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //cidade
                if (!string.IsNullOrEmpty(cidade))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("Cidade", cidade);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                //pais
                if (!string.IsNullOrEmpty(pais))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("Pais", pais);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                //OutroMidia
                if (!string.IsNullOrEmpty(outromidia))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<ClienteModel>("OutroMidia", outromidia);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<ClienteModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                //idoperador
                if (idoperador != null && idoperador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<ClienteModel>("IdOperador", idoperador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                //idempresa
                if (idempresa != null && idempresa.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<ClienteModel>("IdEmpresa", idempresa);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }

                //idgrupoempresa
                if (idgrupoempresa != null && idgrupoempresa.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<ClienteModel>("IdGrupoEmpresa", idgrupoempresa);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (blacklist != null)
                {
                    var boolFilter = _dbcontext.bdDepartamento;
                    if (boolFilter != null)
                        query = query.Where(o => o.BlackList == blacklist);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Cliente", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<ClienteModel>>> CreateCliente(
            [FromQuery] string nome = "",
            [FromQuery] string? cpf = "",
            [FromQuery] string? rg = "",
            [FromQuery] string? cnh = "",
            [FromQuery] string celular = "",
            [FromQuery] string? telefone = "",
            [FromQuery] string email = "",
            [FromQuery] string? cnhcategoria = "",
            [FromQuery] string? cnhestadoemissor = "",
            [FromQuery] string? cnhrestricoes = "",
            [FromQuery] string? cnhrenach = "",
            [FromQuery] string? cnhcodseguranca = "",
            [FromQuery] string cep = "",
            [FromQuery] string? logradouro = "",
            [FromQuery] string? numeroendereco = "",
            [FromQuery] string? bairro = "",
            [FromQuery] string? complemento = "",
            [FromQuery] string? estado = "",
            [FromQuery] string? cidade = "",
            [FromQuery] string? pais = "",
            [FromQuery] string? outromidia = "",
            [FromQuery] bool? blacklist = false,
            [FromQuery] int idmidia = 0,
            [FromQuery] int idoperador = 0,
            [FromQuery] int idempresa = 0,
            [FromQuery] int? idgrupoempresa = null,
            [FromQuery] int idcriador = 0,
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] DateTime? dataalteracao = null,
            [FromQuery] DateTime? datanascimento = null,
            [FromQuery] DateTime? cnhvalidade = null,
            [FromQuery] DateTime? cnhdataemissao = null)
        {
            try
            {
                if (
                    !string.IsNullOrEmpty(nome) &&
                    !string.IsNullOrEmpty(celular) &&
                    !string.IsNullOrEmpty(email) &&
                    !string.IsNullOrEmpty(cep) &&
                    !string.IsNullOrEmpty(datacriacao.ToString()) &&
                    idmidia != 0 &&
                    idoperador != 0 &&
                    idempresa != 0 &&
                    idcriador != 0)
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Now;

                    var newCliente = new ClienteModel
                    {
                        NomeCliente = nome,
                        CPFCliente = string.IsNullOrEmpty(cpf) ? null : cpf,
                        RGOCliente = string.IsNullOrEmpty(rg) ? null : rg,
                        CNHCliente = string.IsNullOrEmpty(cnh) ? null : cnh,
                        DataNascimentoCliente = string.IsNullOrEmpty(datanascimento.ToString()) ? null : datanascimento,
                        Celular = celular,
                        Telefone = string.IsNullOrEmpty(telefone) ? null : telefone,
                        EmailCliente = email,
                        CNHValidade = string.IsNullOrEmpty(cnhvalidade.ToString()) ? null : cnhvalidade,
                        CNHCategoria = string.IsNullOrEmpty(cnhcategoria) ? null : cnhcategoria,
                        CNHDataEmissao = string.IsNullOrEmpty(cnhvalidade.ToString()) ? null : cnhvalidade,
                        CNHEstadoEmissor = string.IsNullOrEmpty(cnhestadoemissor) ? null : cnhestadoemissor,
                        CNHRestricoes = string.IsNullOrEmpty(cnhrestricoes) ? null : cnhrestricoes,
                        CNHRenach = string.IsNullOrEmpty(cnhrenach) ? null : cnhrenach,
                        CNHCodSeguranca = string.IsNullOrEmpty(cnhcodseguranca) ? null : cnhcodseguranca,
                        CEP = cep,
                        Logradouro = string.IsNullOrEmpty(logradouro) ? null : logradouro,
                        NumeroEndereco = string.IsNullOrEmpty(numeroendereco) ? null : numeroendereco,
                        Bairro = string.IsNullOrEmpty(bairro) ? null : bairro,
                        Complemento = string.IsNullOrEmpty(complemento) ? null : complemento,
                        Estado = string.IsNullOrEmpty(estado) ? null : estado,
                        Cidade = string.IsNullOrEmpty(cidade) ? null : cidade,
                        Pais = string.IsNullOrEmpty(pais) ? null : pais,
                        DataCriacao = datacriacao,
                        DataAlteracao = string.IsNullOrEmpty(dataalteracao.ToString()) ? null : dataalteracao,
                        BlackList = blacklist,
                        IdMidia = idmidia,
                        OutroMidia = string.IsNullOrEmpty(outromidia) ? null : outromidia,
                        IdOperador = idoperador,
                        IdEmpresa = idempresa,
                        IdGrupoEmpresa = idgrupoempresa != null ? null : idgrupoempresa,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdCliente.Add(newCliente);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdCliente.ToListAsync());
                }
                else
                {
                    string? props;

                    props = !string.IsNullOrEmpty(nome) ? null : Concat.ConcatString("Nome");
                    props = !string.IsNullOrEmpty(celular) ? null : Concat.ConcatString("Celular");
                    props = !string.IsNullOrEmpty(email) ? null : Concat.ConcatString("E-mail");
                    props = !string.IsNullOrEmpty(cep) ? null : Concat.ConcatString("CEP");
                    props = idmidia != 0 ? null : Concat.ConcatString("Midia de Entrada");
                    props = idoperador != 0 ? null : Concat.ConcatString("Operador Responsável");
                    props = idempresa != 0 ? null : Concat.ConcatString("Empresa");

                    return BadRequest(CRMMessages.PostMissingItems("Cliente", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Cliente", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<ClienteModel>> UpdateCliente(ClienteModel clienteModel,
            [FromQuery] int? ids,
            [FromQuery] string? nome,
            [FromQuery] string? cpf,
            [FromQuery] string? rg,
            [FromQuery] string? cnh,
            [FromQuery] string? celular,
            [FromQuery] string? telefone,
            [FromQuery] string? email,
            [FromQuery] string? cnhcategoria,
            [FromQuery] string? cnhestadoemissor,
            [FromQuery] string? cnhrestricoes,
            [FromQuery] string? cnhrenach,
            [FromQuery] string? cnhcodseguranca,
            [FromQuery] string? cep,
            [FromQuery] string? logradouro,
            [FromQuery] string? numeroendereco,
            [FromQuery] string? bairro,
            [FromQuery] string? complemento,
            [FromQuery] string? estado,
            [FromQuery] string? cidade,
            [FromQuery] string? pais,
            [FromQuery] string? outromidia,
            [FromQuery] bool? blacklist,
            [FromQuery] int? idmidia,
            [FromQuery] int? idoperador,
            [FromQuery] int? idempresa,
            [FromQuery] int? idgrupoempresa,
            [FromQuery] int? idcriador,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? dataalteracao,
            [FromQuery] DateTime? datanascimento,
            [FromQuery] DateTime? cnhvalidade,
            [FromQuery] DateTime? cnhdataemissao)
        {
            try
            {
                var bdCliente = await _dbcontext.bdCliente.FindAsync(clienteModel.IdCliente);
                if (bdCliente == null)
                    return BadRequest(CRMMessages.NotFind("Cliente"));
                else
                {
                    bdCliente.NomeCliente = nome ?? bdCliente.NomeCliente;
                    bdCliente.CPFCliente = cpf ?? bdCliente.CPFCliente;
                    bdCliente.RGOCliente = rg ?? bdCliente.RGOCliente;
                    bdCliente.CNHCliente = cnh ?? bdCliente.CNHCliente;
                    bdCliente.DataNascimentoCliente = datanascimento ?? bdCliente.DataNascimentoCliente;
                    bdCliente.Celular = celular ?? bdCliente.Celular;
                    bdCliente.Telefone = telefone ?? bdCliente.Telefone;
                    bdCliente.EmailCliente = email ?? bdCliente.EmailCliente;
                    bdCliente.CNHValidade = cnhvalidade ?? bdCliente.CNHValidade;
                    bdCliente.CNHCategoria = cnhcategoria ?? bdCliente.CNHCategoria;
                    bdCliente.CNHDataEmissao = cnhdataemissao ?? bdCliente.CNHDataEmissao;
                    bdCliente.CNHEstadoEmissor = cnhestadoemissor ?? bdCliente.CNHEstadoEmissor;
                    bdCliente.CNHRestricoes = cnhrestricoes ?? bdCliente.CNHRestricoes;
                    bdCliente.CNHRenach = cnhrenach ?? bdCliente.CNHRenach;
                    bdCliente.CNHCodSeguranca = cnhcodseguranca ?? bdCliente.CNHCodSeguranca;
                    bdCliente.CEP = cep ?? bdCliente.CEP;
                    bdCliente.Logradouro = logradouro ?? bdCliente.Logradouro;
                    bdCliente.NumeroEndereco = numeroendereco ?? bdCliente.NumeroEndereco;
                    bdCliente.Bairro = bairro ?? bdCliente.Bairro;
                    bdCliente.Complemento = complemento ?? bdCliente.Complemento;
                    bdCliente.Estado = estado ?? bdCliente.Estado;
                    bdCliente.Cidade = cidade ?? bdCliente.Cidade;
                    bdCliente.Pais = pais ?? bdCliente.Pais;
                    bdCliente.DataAlteracao = dataalteracao ?? bdCliente.DataAlteracao;
                    bdCliente.BlackList = blacklist ?? bdCliente.BlackList;
                    bdCliente.IdMidia = idmidia ?? bdCliente.IdMidia;
                    bdCliente.OutroMidia = outromidia ?? bdCliente.OutroMidia;
                    bdCliente.IdOperador = idoperador ?? bdCliente.IdOperador;
                    bdCliente.IdEmpresa = idempresa ?? bdCliente.IdEmpresa;
                    bdCliente.IdGrupoEmpresa = idgrupoempresa ?? bdCliente.IdGrupoEmpresa;
                    bdCliente.IdCriador = idcriador ?? bdCliente.IdCriador;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(bdCliente);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Cliente", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
