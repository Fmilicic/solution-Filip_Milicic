namespace ProductMiddleware.Application.Interfaces;

public interface IProductQueryCacheInvalidator
{
    void InvalidateSearchAndFilterCache();
}
