using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class TK_NewOperador
    {
        [Key]
        public int IdTkNewOperador { get; set; }
        public int IdOperador { get; set; }
        public string? CpfNewOperador { get; set; }
        public int IdNewOperador { get; set; }
        public string? Token { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public string Email { get; set; }
    }
}
