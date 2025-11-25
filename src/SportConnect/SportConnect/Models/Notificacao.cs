using Microsoft.EntityFrameworkCore;
using SportConnect.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 
namespace SportConnect.Models

{

    [Table("Notificacoes")]

    public class Notificacao

    {

        [Key]

        public int Id { get; set; }



        public int UsuarioId { get; set; }



        public string Mensagem { get; set; }



        public DateTimeOffset DataEnvio { get; set; }



        public string Lida { get; set; }

    }

}
