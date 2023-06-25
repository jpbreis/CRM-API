using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class NivelAcessoModel
    {
        [Key]
        public int IdNivelAcesso { get; set; }
        public string DescricaoNivel { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool? Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
