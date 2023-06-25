using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class ProdutoModel
    {
        [Key]
        public string DescricaoProduto { get; set; }
        public string NomeProduto { get; set; }
        public int IdProduto { get; set; }
        public int IdCriador { get; set; }
        public double? ValorMinimo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool Ativo { get; set; }
    }
}
