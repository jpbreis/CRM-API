using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public  CargoController(DataContext context)
        {
            _dbcontext = context;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<CargoModel>>> GetCargo(
            [FromQuery] int[]? ids,
            [FromQuery] string? nome,
            [FromQuery] decimal[]? salario,
            [FromQuery] bool? ativo,
            [FromQuery] int[]? idscriacao,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado,
            [FromQuery] DateTime? datacriacaoBetween,
            [FromQuery] DateTime? datadesativadoBetween)
        {
            try
            {
                IQueryable<CargoModel> query = _dbcontext.bdCargo;

                #region Filter
                var hasFilter =
                    ids != null ||
                    nome != null||
                    salario != null ||
                    datacriacao != null ||
                    datadesativado != null ||
                    ativo != null ||
                    idscriacao != null ||
                    datacriacaoBetween != null ||
                    datadesativadoBetween != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdCargo
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdCargo
                        .Where(x => ids.Contains(x.IdCargo))
                        .ToListAsync();
                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    CargoModel model = new CargoModel
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
                    ? EndPointDataFilter.CreateDataFilter<CargoModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<CargoModel>(startDate, endDate, "DataDesativado") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region String
                if (!string.IsNullOrEmpty(nome))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<CargoModel>("NomeCargo", nome);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Decimal
                if (salario != null && salario.Length > 0)
                {
                    var decimalFilter = EndPointDecimalFilter.CreateDoublePropertyFilter<CargoModel>("SalarioBase", salario);
                    if (decimalFilter != null)
                        query = query.Where(decimalFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idscriacao != null && idscriacao.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<CargoModel>("IdCriador", idscriacao);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region Bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdCargo;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Cargo", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<CargoModel>>> CreateGargo(
            [FromQuery] string nome = "",
            [FromQuery] decimal? salario = 0,
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] bool ativo = true,
            [FromQuery] int idcriador = 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(nome))
                {
                    var newCargo = new CargoModel
                    {
                        NomeCargo = nome,
                        SalarioBase = salario != 0 ? null : salario,
                        DataCriacao = datacriacao,
                        Ativo = ativo,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdCargo.Add(newCargo);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdCargo.ToListAsync());
                }
                else
                {
                    string props = !string.IsNullOrEmpty(nome) ? null : Concat.ConcatString("Nome");

                    return BadRequest(CRMMessages.PostMissingItems("Cargo", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError($"Cargo: {nome}", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<CargoModel>> UpdateCargo(CargoModel cargoM,
            [FromQuery] string? nome,
            [FromQuery] decimal? salario,
            [FromQuery] bool? ativo,
            [FromQuery] int? idscriacao,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var dbCargo = await _dbcontext.bdCargo.FindAsync(cargoM.IdCargo);
                if (dbCargo == null)
                    return BadRequest(CRMMessages.NotFind("Cargo"));
                else
                {
                    dbCargo.NomeCargo = nome ?? dbCargo.NomeCargo;
                    dbCargo.SalarioBase = salario ?? dbCargo.SalarioBase;
                    dbCargo.DataCriacao = datacriacao ?? dbCargo.DataCriacao;
                    dbCargo.DataDesativado = datadesativado ?? dbCargo.DataDesativado;
                    dbCargo.Ativo = ativo ?? dbCargo.Ativo;
                    dbCargo.IdCriador = idscriacao ?? dbCargo.IdCriador;

                    await _dbcontext.SaveChangesAsync();

                    return Ok(dbCargo);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Cargo", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
