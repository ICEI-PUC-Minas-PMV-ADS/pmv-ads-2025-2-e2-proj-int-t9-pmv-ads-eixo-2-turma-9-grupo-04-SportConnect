using CriarGrupo.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SportConnect.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o nome!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o CPF!")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a data de nascimento!")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateOnly DataDeNascimento { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o estado!")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a cidade!")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a senha!")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        public ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
        public ICollection<Evento> EventosCriados { get; set; } = new List<Evento>();
    }
}