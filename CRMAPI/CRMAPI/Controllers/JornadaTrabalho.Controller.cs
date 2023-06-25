using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JornadaTrabalhoController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public JornadaTrabalhoController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region HttpGet
        [HttpGet]
        public async Task<ActionResult<List<JornadaTrabalhoModel>>> GetJornadaTrabalho(
            [FromQuery] int[]? ids,
            [FromQuery] string? descricao,
            //[FromQuery] TimeSpan? inicio,
            //[FromQuery] TimeSpan? termino,
            //[FromQuery] TimeSpan? duracao,
            //[FromQuery] TimeSpan? intervalo,
            [FromQuery] string? periodo,
            [FromQuery] bool? seg,
            [FromQuery] bool? ter,
            [FromQuery] bool? qua,
            [FromQuery] bool? qui,
            [FromQuery] bool? sex,
            [FromQuery] bool? sab,
            [FromQuery] bool? dom,
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
                IQueryable<JornadaTrabalhoModel> query = _dbcontext.bdJornadaTrabalho;

                #region Filter
                var hasFilter =
                    ids != null ||
                    !string.IsNullOrEmpty(descricao) ||
                    //inicio != null ||
                    //termino != null ||
                    //duracao != null ||
                    //intervalo != null ||
                    !string.IsNullOrEmpty(periodo) ||
                    seg != null ||
                    ter != null ||
                    qua != null ||
                    qui != null ||
                    sex != null ||
                    sab != null ||
                    dom != null ||
                    ativo != null ||
                    datacriacao != null ||
                    dataalteracao != null ||
                    datadesativado != null ||
                    datacriacaoBetween != null ||
                    dataalteracaoBetween != null ||
                    datadesativadoBetween != null ||
                    idcriador != null;

                if (!hasFilter)
                    return Ok(await query.ToListAsync());
                #endregion

                #region IdJornadaTrabalho
                if (ids != null && ids.Length > 0)
                {
                    var TabeleaID = await _dbcontext.bdJornadaTrabalho
                        .Where(x => ids.Contains(x.IdJornadaTrabalho))
                        .ToListAsync();

                    return Ok(TabeleaID);
                }
                #endregion

                #region Data
                if (datacriacao != null || datadesativado != null || dataalteracao != null)
                {
                    var startDate = (DateTime?)null;
                    var endDate = (DateTime?)null;

                    JornadaTrabalhoModel model = new JornadaTrabalhoModel
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
                    ? EndPointDataFilter.CreateDataFilter<JornadaTrabalhoModel>(startDate, endDate, "DataCriacao")
                    : datadesativado != null || datadesativadoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<JornadaTrabalhoModel>(startDate, endDate, "DataDesativado")
                    : dataalteracao != null || dataalteracaoBetween != null
                    ? EndPointDataFilter.CreateDataFilter<JornadaTrabalhoModel>(startDate, endDate, "DataAlteracao") : null;

                    if (dataFilter != null)
                        query = query.Where(dataFilter).AsQueryable();
                }
                #endregion

                #region TimeSpan
                //A Fazer
                #endregion

                #region String
                if (!string.IsNullOrEmpty(descricao))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<JornadaTrabalhoModel>("DescricaoJornada", descricao);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }

                if (!string.IsNullOrEmpty(periodo))
                {
                    var stringFilter = EndPointStringFilter.CreateStringPropertyFilter<JornadaTrabalhoModel>("PeriodoJornada", periodo);

                    if (stringFilter != null)
                        query = query.Where(stringFilter).AsQueryable();
                }
                #endregion

                #region Ids
                if (idcriador != null && idcriador.Length > 0)
                {
                    var idsFilter = EndPointIntFilter.CreateIntPropertyFilter<JornadaTrabalhoModel>("IdCriador", idcriador);
                    if (idsFilter != null)
                        query = query.Where(idsFilter).AsQueryable();
                }
                #endregion

                #region bool
                if (ativo != null)
                {
                    var boolFilter = _dbcontext.bdJornadaTrabalho;
                    if (boolFilter != null)
                        query = query.Where(o => o.Ativo == ativo);
                }

                if (seg != null)
                {
                    var boolFilter = _dbcontext.bdJornadaTrabalho;
                    if (boolFilter != null)
                        query = query.Where(o => o.SEG == seg);
                }

                if (ter != null)
                {
                    var boolFilter = _dbcontext.bdJornadaTrabalho;
                    if (boolFilter != null)
                        query = query.Where(o => o.TER == ter);
                }

                if (qua != null)
                {
                    var boolFilter = _dbcontext.bdJornadaTrabalho;
                    if (boolFilter != null)
                        query = query.Where(o => o.QUA == qua);
                }

                if (qui != null)
                {
                    var boolFilter = _dbcontext.bdJornadaTrabalho;
                    if (boolFilter != null)
                        query = query.Where(o => o.QUI == qui);
                }

                if (sex != null)
                {
                    var boolFilter = _dbcontext.bdJornadaTrabalho;
                    if (boolFilter != null)
                        query = query.Where(o => o.SEX == sex);
                }

                if (sab != null)
                {
                    var boolFilter = _dbcontext.bdJornadaTrabalho;
                    if (boolFilter != null)
                        query = query.Where(o => o.SAB == sab);
                }

                if (dom != null)
                {
                    var boolFilter = _dbcontext.bdJornadaTrabalho;
                    if (boolFilter != null)
                        query = query.Where(o => o.DOM == dom);
                }
                #endregion

                return Ok(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.GetError("Jornada de Trabalho", ex.ToString()));
            }
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public async Task<ActionResult<List<JornadaTrabalhoModel>>> CreateJornadaTrabalho(
            [FromQuery] string descricao = "",
            [FromQuery] TimeSpan inicio = default,
            [FromQuery] TimeSpan termino = default,
            [FromQuery] TimeSpan duracao = default,
            [FromQuery] TimeSpan intervalo = default,
            [FromQuery] bool seg = false,
            [FromQuery] bool ter = false,
            [FromQuery] bool qua = false,
            [FromQuery] bool qui = false,
            [FromQuery] bool sex = false,
            [FromQuery] bool sab = false,
            [FromQuery] bool dom = false,
            [FromQuery] bool ativo = true,
            [FromQuery] DateTime datacriacao = default,
            [FromQuery] int idcriador = 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(descricao) &&
                    inicio != default &&
                    termino != default &&
                    duracao != default &&
                    intervalo != default &&
                    seg != null &&
                    ter != null &&
                    qua != null &&
                    qui != null &&
                    sex != null &&
                    sab != null &&
                    dom != null &&
                    ativo != null)
                {
                    if (datacriacao == default)
                        datacriacao = DateTime.Today;

                    var newJornadaTrabalho = new JornadaTrabalhoModel
                    {
                        DescricaoJornada = descricao,
                        InicioJornada = inicio,
                        TerminoJornada = termino,
                        DuracaoJornada = duracao,
                        Intervalo = intervalo,
                        SEG = seg,
                        TER = ter,
                        QUA = qua,
                        QUI = qui,
                        SEX = sex,
                        SAB = sab,
                        DOM = dom,
                        Ativo = ativo,
                        DataCriacao = datacriacao,
                        IdCriador = idcriador
                    };

                    _dbcontext.bdJornadaTrabalho.Add(newJornadaTrabalho);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(await _dbcontext.bdJornadaTrabalho.ToListAsync());
                }
                else
                {
                    string? props;
                    props = !string.IsNullOrEmpty(descricao) ? null : Concat.ConcatString("Decrição da Jornada");
                    props = inicio != default ? null : Concat.ConcatString("Horário de Inicio");
                    props = termino != default ? null : Concat.ConcatString("Horário de Termino");
                    props = duracao != default ? null : Concat.ConcatString("Horário de Duração");
                    props = intervalo != default ? null : Concat.ConcatString("Horário de Intervalo");

                    return BadRequest(CRMMessages.PostMissingItems("Jornada de Trabalho", Concat.textFinal));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PostError("Jornada de Trabalho", ex.ToString()));
            }
        }
        #endregion

        #region HttpPut
        [HttpPut]
        public async Task<ActionResult<JornadaTrabalhoModel>> UpdateJornadaTrabalho(JornadaTrabalhoModel jornModel,
            [FromQuery] string? descricao,
            [FromQuery] TimeSpan? inicio,
            [FromQuery] TimeSpan? termino,
            [FromQuery] TimeSpan? duracao,
            [FromQuery] TimeSpan? intervalo,
            [FromQuery] string? periodo,
            [FromQuery] bool? seg,
            [FromQuery] bool? ter,
            [FromQuery] bool? qua,
            [FromQuery] bool? qui,
            [FromQuery] bool? sex,
            [FromQuery] bool? sab,
            [FromQuery] bool? dom,
            [FromQuery] bool? ativo,
            [FromQuery] int? idcriador,
            [FromQuery] DateTime? datacriacao,
            [FromQuery] DateTime? dataalteracao,
            [FromQuery] DateTime? datadesativado)
        {
            try
            {
                var dbJornTrab = await _dbcontext.bdJornadaTrabalho.FindAsync(jornModel.IdJornadaTrabalho);
                if (dbJornTrab == null)
                    return BadRequest(CRMMessages.NotFind("Jornada De Trabalho"));
                else
                {
                    dbJornTrab.DescricaoJornada = descricao ?? dbJornTrab.DescricaoJornada;
                    dbJornTrab.InicioJornada = inicio ?? dbJornTrab.InicioJornada;
                    dbJornTrab.TerminoJornada = termino ?? dbJornTrab.TerminoJornada;
                    dbJornTrab.DuracaoJornada = duracao ?? dbJornTrab.DuracaoJornada;
                    dbJornTrab.Intervalo = intervalo ?? dbJornTrab.Intervalo;
                    dbJornTrab.PeriodoJornada = periodo ?? dbJornTrab.PeriodoJornada;
                    dbJornTrab.SEG = seg ?? dbJornTrab.SEG;
                    dbJornTrab.TER = ter ?? dbJornTrab.TER;
                    dbJornTrab.QUA = qua ?? dbJornTrab.QUA;
                    dbJornTrab.QUI = qui ?? dbJornTrab.QUI;
                    dbJornTrab.SEX = sex ?? dbJornTrab.SEX;
                    dbJornTrab.SAB = sab ?? dbJornTrab.SAB;
                    dbJornTrab.DOM = dom ?? dbJornTrab.DOM;
                    dbJornTrab.Ativo = ativo ?? dbJornTrab.Ativo;
                    dbJornTrab.DataCriacao = datacriacao ?? dbJornTrab.DataCriacao;
                    dbJornTrab.DataAlteracao = dataalteracao ?? dbJornTrab.DataAlteracao;
                    dbJornTrab.DataDesativado = datadesativado ?? dbJornTrab.DataDesativado;
                    dbJornTrab.IdCriador = idcriador ?? dbJornTrab.IdCriador;

                    await _dbcontext.SaveChangesAsync();
                    return Ok(dbJornTrab);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(CRMMessages.PutError("Jornada De Trabalho", ex.ToString()));
            }
        }
        #endregion

        #region HttpDelete
        #endregion
    }
}
