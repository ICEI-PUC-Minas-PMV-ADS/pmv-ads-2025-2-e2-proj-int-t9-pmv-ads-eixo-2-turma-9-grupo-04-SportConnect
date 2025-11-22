using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Composition;

namespace SportConnect.Models
{
    [Table("Modalidades")]
    public class Modalidade
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}