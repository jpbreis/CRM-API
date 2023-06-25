using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class FormaPagamentoModel
    {
        [Key]
        public int IdFormaPagamento { get; set; }
        public string FormaPagamento { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
