using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportConnect.Models
{
    [Table("Participacoes")]
    public class Participacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }   // FK para Usuarios

        [Required]
        public int GrupoId { get; set; }     // FK para Grupos

        [Required]
        [MaxLength(30)]
        public string StatusParticipacao { get; set; } = "Inscrito"; // "Inscrito" ou "Lista de Espera"

        [Required]
        public DateTimeOffset DataInscricao { get; set; } = DateTimeOffset.UtcNow;
    }
}

