using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class ClienteModel
    {
        [Key]
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public string CPFCliente { get; set; }
        public string RGOCliente { get; set; }
        public string CNHCliente { get; set; }
        public DateTime? DataNascimentoCliente { get; set; }
        public string Celular { get; set; }
        public string Telefone { get; set; }
        public string EmailCliente { get; set; }
        public DateTime? CNHValidade { get; set; }
        public string CNHCategoria { get; set; }
        public DateTime? CNHDataEmissao { get; set; }
        public string CNHEstadoEmissor { get; set; }
        public string CNHRestricoes { get; set; }
        public string CNHRenach { get; set; }
        public string CNHCodSeguranca { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string NumeroEndereco { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Pais { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool? BlackList { get; set; }
        public int IdMidia { get; set; }
        public string OutroMidia { get; set; }
        public int IdOperador { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdGrupoEmpresa { get; set; }
        public int IdCriador { get; set;}
    }
}
