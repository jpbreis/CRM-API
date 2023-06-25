using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class MidiaModel
    {
        [Key]
        public int IdMidia { get; set; }
        public string NomeMidia { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
