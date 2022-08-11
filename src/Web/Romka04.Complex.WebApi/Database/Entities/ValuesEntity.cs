using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Romka04.Complex.WebApi.Database.Entities;

[Table("Values")]
public class ValuesEntity
{
    [Key]
    public int Number { get; set; }
}