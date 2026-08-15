using FactoryManager.Application.DTOs.Products;
using FactoryManager.Domain.Entities;

public static class ProductMapper
{
    public static ProductResponse ToProductResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name
        };
    }
}