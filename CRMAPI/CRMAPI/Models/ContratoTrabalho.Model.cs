using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class ContratoTrabalhoModel
    {
        [Key]
        public int IdContratoTrabalho { get; set; }
        public string TipoContrato { get; set; }
        public string DescricaoContrato { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
