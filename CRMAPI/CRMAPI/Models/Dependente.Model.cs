using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class DependenteModel
    {
        [Key]
        public int IdDependente { get; set; }
        public int IdOperador { get; set; }
        public string NomeDependente { get; set; }
        public DateTime DataNascimentoDependente { get; set; }
        public int IdParentesco { get; set; }
        public string Genero { get; set; }
        public string CPFDependente { get; set; }
        public string? RGDependente { get; set; }
        public string CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? NumeroEndereco { get; set; }
        public string? Bairro { get; set; }
        public string? Complemento { get; set; }
        public string? Estado { get; set; }
        public string? Cidade { get; set; }
        public string Pais { get; set; }
        public string? EstadoCivil { get; set; }
        public string Celular { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
