using System.ComponentModel.DataAnnotations;

namespace CRMAPI.Models
{
    public class JornadaTrabalhoModel
    {
        [Key]
        public int IdJornadaTrabalho { get; set; }
        public string DescricaoJornada { get; set; }
        public TimeSpan InicioJornada { get; set; }
        public TimeSpan TerminoJornada { get; set; }
        public TimeSpan DuracaoJornada { get; set; }
        public TimeSpan Intervalo { get; set; }
        public string PeriodoJornada { get; set; }
        public bool SEG { get; set; }
        public bool TER { get; set; }
        public bool QUA { get; set; }
        public bool QUI { get; set; }
        public bool SEX { get; set; }
        public bool SAB { get; set; }
        public bool DOM { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataDesativado { get; set; }
        public int IdCriador { get; set; }
    }
}
