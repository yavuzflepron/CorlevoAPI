using Microsoft.AspNetCore.Mvc;

namespace API
{
    public interface IProductsController
    {
        ActionResult<IEnumerable<Product>?> GetAll();
        ActionResult<Product?> GetById(Guid id);
        ActionResult<IEnumerable<Product>?> Search([FromQuery] SearchProductRequest request);
        ActionResult<Product?> Create([FromBody] AddProductRequest request);
        ActionResult<Product?> Update(Guid id, [FromBody] UpdateProductRequest request);
        ActionResult Delete(Guid id);
    }
}