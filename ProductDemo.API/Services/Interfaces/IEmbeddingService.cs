using ProductDemo.API.Models;

namespace ProductDemo.API.Services.Interfaces
{
    public interface IEmbeddingService
    {
        Task CreateProductEmbeddingAsync(Product product);
        Task<List<Product>> SearchProductsAsync(string query, int topN = 5);
        Task DeleteProductEmbeddingAsync(int productId);
    }
}