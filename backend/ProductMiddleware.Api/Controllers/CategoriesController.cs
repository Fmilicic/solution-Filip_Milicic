using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMiddleware.Api.Contracts;
using ProductMiddleware.Application.Interfaces;

namespace ProductMiddleware.Api.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<CategoryResponse>>> GetCategories(CancellationToken cancellationToken = default)
    {
        try
        {
            var categories = await _categoryService.GetCategoriesAsync(cancellationToken);
            var response = categories
                .Select(category => new CategoryResponse
                {
                    Slug = category.Slug,
                    Name = category.Name
                })
                .ToList();

            return Ok(response);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Upstream categories request failed");
            return StatusCode(StatusCodes.Status502BadGateway, new ApiErrorResponse { Message = "Upstream category source is unavailable." });
        }
    }
}
