using System.ComponentModel.DataAnnotations;

namespace CRMAPI
{
    public class Operador
    {
        [Key]
        public int IdOperador { get; set; }
        public string? Nome { get; set; }
        public string? DataNascimento { get; set; }
        public string? Rg { get; set; }
        public string? Cpf { get; set; }
        public string? Cep { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Complemento { get; set; }
        public string? Estado { get; set; }
        public string? Cidade { get; set; }
        public string? Pais { get; set; }
        public string? Celular { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? EstadoCivil { get; set; }
        public int? Dependentes { get; set; }
        public int? Cargo { get; set; }
        public int? Departamento { get; set; }
        public int? Supervisor { get; set; }
        public string? DataAdmissao { get; set; }
        public string? DataDemissao { get; set; }
        public int? Tipocontrato { get; set; }
        public int? Regimetrabalho { get; set; }
        public int? JornadaTrabalho { get; set; }
        public int? Salario { get; set; }
    }
}
