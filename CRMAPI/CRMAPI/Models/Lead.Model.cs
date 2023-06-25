using System.ComponentModel.DataAnnotations;
using static CRMAPI.Controllers.GlobalControllers;

namespace CRMAPI.Models
{
    public class LeadModel
    {
        [Key]
        public int IdLead { get; set; }
        public string? NomeLead { get; set; }
        public string? TelefoneLead { get; set; }
        public string? CelularLead { get; set; }
        public string? EmailLead { get; set; }
        public int? IdOperador { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdGrupoEmpresa { get; set; }
        public int? IdCliente { get; set; }
        public int? IdAcompanhamento { get; set; }
        public int? IdMidia { get; set; }
        public string? OutroMidia { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataInteracao { get; set; }
        public int IdCriador { get; set; }
    }
}
