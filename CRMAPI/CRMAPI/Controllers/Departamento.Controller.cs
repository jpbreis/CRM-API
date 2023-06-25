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
    public class DepartamentoController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public DepartamentoController(DataContext context)
        {
            _dbcontext = context;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<DepartamentoModel>>> GetDepartamento(
            [FromQuery] int[]? ids,
            [FromQuery] string? nome,
            [FromQuery] string? descricao,
            [FromQuery] bool? ativo,
            [FromQuery] int[]? idscriacao,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? datadesativadoBetween)
        {
            try
            {
                IQueryable<DepartamentoModel> query = _dbcontext.bdDepartamento;

                #region Filter
                bool hasFilter =
                    ids != null ||
                    !string.IsNullOrEmpty(nome) ||
                    !string.IsNullOrEmpty(descricao) ||
                    ativo != null ||
                    idscriacao != null ||
                    datacriacao != null ||
                    datadesativado != null;


                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdDepartamento
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdDepartamento
                        .Where(x => ids.Contains(x.IdDepartamento))
                        .ToListAsync();

                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
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
                    else if (datadesativado != null || datadesativadoBetween != null)
                    {
                        startDate = datadesativado.HasValue ? datadesativado.Value.Date : null;
                        endDate = datadesativadoBetween.HasValue ? datadesativadoBetween.Value.Date : null;
                    }

                    var dataFilter = datacriacao != null || datacriacaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<DepartamentoModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<DepartamentoModel>(startDate, endDate, "DataDesativado") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(nome))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DepartamentoModel>("NomeDepartamento", nome);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                if (!string.IsNullOrEmpty(descricao))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<DepartamentoModel>("DescricaoDepartamento", descricao);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idscriacao != null && idscriacao.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<DepartamentoModel>("IdCriador", idscriacao);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdDepartamento;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Departamento", ex.ToString()));
            }
        }

        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<DepartamentoModel>>> CreateDepartamento(
           [FromQuery] string nome = "",
           [FromQuery] string descricao = "",
           [FromQuery] bool ativo = true,
           [FromQuery] int idscriacao = 0,
           [FromQuery] DateTime datacriacao = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(descricao))
                {
                    var newDepartamento = new DepartamentoModel
                    {
                        NomeDepartamento = nome,
                        DescricaoDepartamento = descricao,
                        Ativo = ativo,
                        IdCriador = idscriacao,
                        DataCriacao = datacriacao
                    };

                    _dbcontext.bdDepartamento.Add(newDepartamento);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdDepartamento.ToListAsync());
                }
                else
                {
                    string? props;
                    props = !string.IsNullOrEmpty(nome) ? null : Concat.ConcatString("Nome");
                    props = !string.IsNullOrEmpty(descricao) ? null : Concat.ConcatString("Descrição");

                    return BadRequest(CRMMessages.PostMissingItems("Departamento", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Departamento", ex.ToString()));
            }
        }
        #endregion


        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<DepartamentoModel>> UpdateDepartamento(DepartamentoModel departamento,
            [FromQuery] string? nome,
            [FromQuery] string? descricao,
            [FromQuery] bool? ativo,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var bdDepart = await _dbcontext.bdDepartamento.FindAsync(departamento.IdDepartamento);
                if (bdDepart == null)
                    return BadRequest(CRMMessages.NotFind("Departamento"));
                else
                {
                    bdDepart.NomeDepartamento = nome ?? bdDepart.NomeDepartamento;
                    bdDepart.DescricaoDepartamento = descricao ?? bdDepart.DescricaoDepartamento;
                    bdDepart.DataDesativado = datadesativado ?? bdDepart.DataDesativado;
                    bdDepart.Ativo = ativo ?? bdDepart.Ativo;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(bdDepart);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError($"Departamento: {departamento.IdDepartamento} - {departamento.NomeDepartamento}", ex.ToString()));
            }
        }
        #endregion


        #region HttpDelete
        #endregion
    }
}
