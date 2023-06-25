using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models

{
    public class EmpresaModel
    {
        [Key]
        public int IdEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public string CNPJEmpresa { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
