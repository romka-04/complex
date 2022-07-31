namespace Romka04.Complex.Core;

public static class FabCalculator
{
    public static int Fab(int index)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (index == 0)
            return 0;

        if (index <= 2)
            return 1;

        return Fab(index - 1) + Fab(index - 2);
    }
}