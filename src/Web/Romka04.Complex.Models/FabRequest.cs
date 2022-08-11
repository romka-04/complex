using System.ComponentModel.DataAnnotations;

namespace Romka04.Complex.Models;

public class FabRequest
{
    [Required]
    public int? Index { get; set; }
}

public record FabResponse(int Index, int Value) {}