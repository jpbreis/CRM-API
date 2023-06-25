using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class AcompanhamentoLeadModel
    {
        [Key]
        public int IdAcompanhamento { get; set; }
        public string? DescricaoAcompanhamento { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool? Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
