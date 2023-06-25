using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class AdicionaisProdutoModel
    {
        [Key]
        public int IdAdicionalProduto { get; set; }
        public int IdProduto { get; set; }
        public string SiglaAdicionalProduto { get; set; }
        public string DescricaoAdicionalProduto { get; set; }
        public double? ValorAdicionalProduto { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public bool? Ativo { get; set; }
        public int IdCriador { get; set; }
    }
}
