using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class OperadoresComplModel
    {
        [Key]
        public int IdOperadorCompl { get; set; }
        public int IdOperador { get; set; }
        public string? RGOperador { get; set; }
        public string CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? NumeroEndereco { get; set; }
        public string? Bairro { get; set; }
        public string? Complemento { get; set; }
        public string? Estado { get; set; }
        public string? Cidade { get; set; }
        public string? Pais { get; set; }
        public string? EstadoCivil { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
