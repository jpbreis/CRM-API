using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class AssociacaoGrupoEmpresaModel
    {
        [Key]
        public int IdAssociacaoGrupo { get; set; }
        public int? IdGrupoEmpresa { get; set; }
        public int? IdEmpresa { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
