namespace Romka04.Complex.Models;

public record FabPair(int Index, int Value)
{
    public static FabPair Parse(string index, string value)
    {
        return new FabPair(int.Parse(index), int.Parse(value));
    }
}