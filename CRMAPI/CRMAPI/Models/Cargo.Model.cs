using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class CargoModel
    {
        [Key]
        public int IdCargo { get; set; }
        public string NomeCargo { get; set; }
        public float? SalarioBase { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool? Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
