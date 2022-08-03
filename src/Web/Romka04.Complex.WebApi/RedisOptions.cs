namespace Romka04.Complex.WebApi;

internal class RedisOptions
{
    public const string Name = "Redis";

    public string Configuration { get; set; } = String.Empty;
    public string PublishChannel { get; set; } = String.Empty;
}