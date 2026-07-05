using ProductMiddleware.Application.Mapping;

namespace ProductMiddleware.Tests.Unit;

public class ProductDescriptionFormatterTests
{
    [Fact]
    public void Truncate_ReturnsFirst100Characters_WhenLongerThanLimit()
    {
        var description = new string('a', 120);

        var result = ProductDescriptionFormatter.Truncate(description);

        Assert.Equal(100, result.Length);
        Assert.Equal(new string('a', 100), result);
    }
}
