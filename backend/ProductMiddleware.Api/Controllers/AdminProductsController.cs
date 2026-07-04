using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMiddleware.Api.Contracts;
using ProductMiddleware.Api.Mapping;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Api.Controllers;

[ApiController]
[Route("api/admin/products")]
[Authorize]
public sealed class AdminProductsController : ControllerBase
{
    private readonly IProductAdminService _productAdminService;

    public AdminProductsController(IProductAdminService productAdminService)
    {
        _productAdminService = productAdminService;
    }

    [HttpPost]
    public async Task<ActionResult<ProductDetailResponse>> CreateProduct(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiErrorResponse { Message = "Invalid product data." });
        }

        try
        {
            var product = await _productAdminService.CreateAsync(ToCreateData(request), cancellationToken);
            return CreatedAtAction(
                nameof(ProductsController.GetProductById),
                "Products",
                new { id = product.Id },
                ProductResponseMapper.ToDetail(product));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new ApiErrorResponse { Message = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDetailResponse>> UpdateProduct(
        int id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiErrorResponse { Message = "Invalid product data." });
        }

        try
        {
            var product = await _productAdminService.UpdateAsync(id, ToUpdateData(request), cancellationToken);
            if (product is null)
            {
                return NotFound(new ApiErrorResponse { Message = "Product not found or cannot be updated." });
            }

            return Ok(ProductResponseMapper.ToDetail(product));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new ApiErrorResponse { Message = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _productAdminService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound(new ApiErrorResponse { Message = "Product not found or cannot be deleted." });
        }

        return NoContent();
    }

    private static CreateProductData ToCreateData(CreateProductRequest request)
    {
        return new CreateProductData
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Category = request.Category
        };
    }

    private static UpdateProductData ToUpdateData(UpdateProductRequest request)
    {
        return new UpdateProductData
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Category = request.Category
        };
    }
}
