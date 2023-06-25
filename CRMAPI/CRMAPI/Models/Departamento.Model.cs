using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class DepartamentoModel
    {
        [Key]
        public int IdDepartamento { get; set; }
        public string? NomeDepartamento { get; set; }
        public string? DescricaoDepartamento { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool? Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
