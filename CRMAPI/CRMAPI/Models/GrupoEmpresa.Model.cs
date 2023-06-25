using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class GrupoEmpresaModel
    {
        [Key]
        public int IdGrupoEmpresa { get; set; }
        public string NomeGrupo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool Ativo { get; set; }
        public int IdCriador { get; set; }
    }   
}
