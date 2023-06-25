using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class VendaModel
    {
        [Key]
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string NumeroEndereco { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Pais { get; set; }
        public int? IdAprovador { get; set; }
        public int IdCliente { get; set; }
        public int IdOperador { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdProduto { get; set; }
        public int IdVenda { get; set; }
        public int? StatusVenda { get; set; }
        public int IdCriador { get; set; }
        public double? ValorVenda { get; set; }
        public double? EntradaVenda { get; set; }
        public DateTime DataAssinatura { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
