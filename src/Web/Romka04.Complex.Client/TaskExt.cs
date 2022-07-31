namespace Romka04.Complex.Client;

public static class TaskExt
{
    public static async Task<IEnumerable<TResult>> WhenAll<TResult>(params Task<TResult>[] tasks)
    {
        var allTasks = Task.WhenAll(tasks);

        try
        {
            return await allTasks;
        }
        catch (Exception)
        {
            // ignore
        }

        throw allTasks.Exception ?? throw new Exception("Should NOT really happen");
    }
    
    public static async Task WhenAll(params Task[] tasks)
    {
        var allTasks = Task.WhenAll(tasks);

        try
        {
            await allTasks;
            return;
        }
        catch (Exception)
        {
            // ignore
        }

        throw allTasks.Exception ?? throw new Exception("Should NOT really happen");
    }
}