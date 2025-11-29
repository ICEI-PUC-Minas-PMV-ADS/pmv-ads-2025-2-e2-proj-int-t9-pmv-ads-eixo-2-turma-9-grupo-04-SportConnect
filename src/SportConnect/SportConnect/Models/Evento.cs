using CriarGrupo.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportConnect.Models
{
    [Table("Eventos")]
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do evento é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição do evento é obrigatória.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A Data e Hora do evento é obrigatória.")]
        [Display(Name = "Data do Evento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime DataEvento { get; set; }

        [Required(ErrorMessage = "A cidade do evento é obrigatória.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O bairro do evento é obrigatório.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "A rua do evento é obrigatória.")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "O número do evento é obrigatório.")]
        public int Numero { get; set; }


        public double? Latitude { get; set; }
        public double? Longitude { get; set; }



        // FK do Grupo

        public int GrupoId { get; set; }

        public Grupo Grupo { get; set; }


        // FK do Criador do Evento
        public int CriadorId { get; set; }

        public Usuario Criador { get; set; }


    }
}
