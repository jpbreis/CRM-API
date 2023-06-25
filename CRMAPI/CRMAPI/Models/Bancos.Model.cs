using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class BancosModel
    {
        [Key]
        public int IdBanco { get; set; }
        public int CodBacen { get; set; }
        public string NomeBanco { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
