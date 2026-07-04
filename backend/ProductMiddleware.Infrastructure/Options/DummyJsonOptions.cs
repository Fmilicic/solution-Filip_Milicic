namespace ProductMiddleware.Infrastructure.Options;

public sealed class DummyJsonOptions
{
    public const string SectionName = "DummyJson";
    public string BaseUrl { get; init; } = "https://dummyjson.com/";
}
