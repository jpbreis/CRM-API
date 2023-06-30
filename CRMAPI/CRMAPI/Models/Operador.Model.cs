using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class OperadorModel
    {
        [Key]
        public int IdOperador { get; set; }
        public string NomeOperador { get; set; }
        public DateTime DataNascimentoOperador { get; set; }
        public string CPFOperador { get; set; }
        public string? EmailOperador { get; set; }
        public string Celular { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }
        public string Genero { get; set; }
        public decimal Salario { get; set; }
        public string Senha { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdGrupoEmpresa { get; set; }
        public int IdCargo { get; set; }
        public int IdDepartamento { get; set; }
        public int IdGestao { get; set; }
        public int IdContratoTrabalho { get; set; }
        public int IdJornadaTrabalho { get; set; }
        public int IdNivelAcesso { get; set; }
        public bool Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
