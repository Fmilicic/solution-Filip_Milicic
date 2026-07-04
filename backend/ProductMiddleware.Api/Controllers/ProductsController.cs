using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMiddleware.Api.Contracts;
using ProductMiddleware.Api.Mapping;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Validation;

namespace ProductMiddleware.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductQueryService _productQueryService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductQueryService productQueryService, ILogger<ProductsController> logger)
    {
        _productQueryService = productQueryService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedProductsResponse>> GetProducts(
        [FromQuery] int page = ProductQueryValidator.DefaultPage,
        [FromQuery] int pageSize = ProductQueryValidator.DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        var validationError = ProductQueryValidator.ValidatePaging(page, pageSize);
        if (validationError is not null)
        {
            return BadRequest(new ApiErrorResponse { Message = validationError });
        }

        try
        {
            var result = await _productQueryService.GetListAsync(page, pageSize, cancellationToken);
            return Ok(ProductResponseMapper.ToPagedResponse(result));
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Upstream product list request failed");
            return StatusCode(StatusCodes.Status502BadGateway, new ApiErrorResponse { Message = "Upstream product source is unavailable." });
        }
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductDetailResponse>> GetProductById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _productQueryService.GetByIdAsync(id, cancellationToken);
            if (product is null)
            {
                return NotFound(new ApiErrorResponse { Message = $"Product {id} was not found." });
            }

            return Ok(ProductResponseMapper.ToDetail(product));
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Upstream product details request failed for id {ProductId}", id);
            return StatusCode(StatusCodes.Status502BadGateway, new ApiErrorResponse { Message = "Upstream product source is unavailable." });
        }
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<ActionResult<PagedProductsResponse>> SearchProducts(
        [FromQuery] string query,
        [FromQuery] int page = ProductQueryValidator.DefaultPage,
        [FromQuery] int pageSize = ProductQueryValidator.DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        var queryValidationError = ProductQueryValidator.ValidateSearchQuery(query);
        if (queryValidationError is not null)
        {
            return BadRequest(new ApiErrorResponse { Message = queryValidationError });
        }

        var pagingValidationError = ProductQueryValidator.ValidatePaging(page, pageSize);
        if (pagingValidationError is not null)
        {
            return BadRequest(new ApiErrorResponse { Message = pagingValidationError });
        }

        try
        {
            var result = await _productQueryService.SearchAsync(query, page, pageSize, cancellationToken);
            return Ok(ProductResponseMapper.ToPagedResponse(result));
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Upstream product search request failed for query {Query}", query);
            return StatusCode(StatusCodes.Status502BadGateway, new ApiErrorResponse { Message = "Upstream product source is unavailable." });
        }
    }

    [HttpGet("filter")]
    [Authorize]
    public async Task<ActionResult<PagedProductsResponse>> FilterProducts(
        [FromQuery] string? category,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int page = ProductQueryValidator.DefaultPage,
        [FromQuery] int pageSize = ProductQueryValidator.DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        var filterValidationError = ProductQueryValidator.ValidateFilter(category, minPrice, maxPrice);
        if (filterValidationError is not null)
        {
            return BadRequest(new ApiErrorResponse { Message = filterValidationError });
        }

        var pagingValidationError = ProductQueryValidator.ValidatePaging(page, pageSize);
        if (pagingValidationError is not null)
        {
            return BadRequest(new ApiErrorResponse { Message = pagingValidationError });
        }

        try
        {
            var result = await _productQueryService.FilterAsync(category, minPrice, maxPrice, page, pageSize, cancellationToken);
            return Ok(ProductResponseMapper.ToPagedResponse(result));
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Upstream product filter request failed");
            return StatusCode(StatusCodes.Status502BadGateway, new ApiErrorResponse { Message = "Upstream product source is unavailable." });
        }
    }
}
