using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase, IProductsController
    {
        private readonly ComplevoContext _context;

        public ProductsController(ComplevoContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "GetAllProducts", Description = "Endpoint for seeking the Products.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<Product>?> GetAll()
        {
            try
            {
                var data = _context.Products.OrderBy(x => x.Name);
                if (!data.Any()) return NoContent();
                return data.ToList();
            }
            catch (Exception ex)
            {
                _context.WriteErrorLog(ex);
                return Problem($"An Error Occurred! Error Message: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }

        /// <summary>
        /// GetProductList - Endpoint for seeking the Products.
        /// </summary>
        /// <param name="SearchText">(optional) Search text for Product Name</param>
        /// <param name="MinPrice">(optional) Filtering the products according to the price</param>
        /// <param name="MaxPrice">(optional) Filtering the products according to the price</param>
        /// <response code="200">Product(s) found according to the search text (if exists)</response>
        /// <response code="204">Product(s) not found according to the search text (if exists)</response>
        /// <returns>Array of Product</returns>
        [HttpGet("/search")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "SearchProduct", Description = "Endpoint for seeking the Products.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<Product>?> Search([FromQuery] SearchProductRequest request)
        {
            try
            {
                var data = _context.Products.Where(
                    x =>
                    (string.IsNullOrEmpty(request.SearchText) || x.Name.ToLower().Contains(request.SearchText.ToLower()))
                    && (!request.MinPrice.HasValue || x.Price >= request.MinPrice.Value)
                    && (!request.MaxPrice.HasValue || x.Price <= request.MaxPrice.Value));
                if (!data.Any()) return NoContent();
                return data.ToList();
            }
            catch (Exception ex)
            {
                _context.WriteErrorLog(ex);
                return Problem($"An Error Occurred! Error Message: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }

        /// <summary>
        /// GetProductDetails - Endpoint for get a single Product by id.
        /// </summary>
        /// <param name="id">(mandatory) Id of the Product</param>
        /// <returns>Product</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "GetProductDetails", Description = "Endpoint for get a single Product by id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product?> GetById(Guid id)
        {
            try
            {
                var data = _context.Products.FirstOrDefault(x => x.Id == id);
                if (data == null) return NotFound(new { Message = "Product not found" });
                return data;
            }
            catch (Exception ex)
            {
                _context.WriteErrorLog(ex);
                return Problem($"An Error Occurred! Error Message: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }

        /// <summary>
        /// AddProduct - Endpoint for add a new Product
        /// </summary>
        /// <param name="request">JSON Request body of the add request</param>
        /// <returns>The added Product with Id</returns>
        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "AddProduct", Description = "Endpoint for add a new Product.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product?> Create([FromBody] AddProductRequest request)
        {
            try
            {
                if (request.Name.Length < 3) return BadRequest(new { Message = "Invalid Product Name" });
                if (request.Price < 0) return BadRequest(new { Message = "Invalid Price" });
                if (_context.Products.Any(x => x.Name == request.Name)) return Conflict(new { Message = $"Product Name '{request.Name}' Already In Use!" });
                var result = _context.Products.Add(new Product() { Name = request.Name, Price = request.Price });
                if (result.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                    _context.SaveChanges();
                return result.Entity;
            }
            catch (Exception ex)
            {
                _context.WriteErrorLog(ex);
                return Problem($"An Error Occurred! Error Message: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }

        /// <summary>
        /// UpdateProduct - Endpoint for updating an existing Product
        /// </summary>
        /// <param name="id">ID value of the Product</param>
        /// <param name="request">JSON Request body of the update request</param>
        /// <returns>Product</returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "UpdateProduct", Description = "Endpoint for updating an existing Product.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product?> Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            try
            {
                var Product = _context.Products.FirstOrDefault(x => x.Id == id);
                if (Product == null) return NotFound(new { Message = "Invalid Id Value" });
                if (_context.Products.Any(x => x.Id != id && x.Name == request.Name)) return Conflict(new { Message = $"Product Name '{request.Name}' Already In Use!" });
                if (!string.IsNullOrEmpty(request.Name)) Product.Name = request.Name;
                if (request.Price.HasValue) Product.Price = request.Price.Value;
                var result = _context.Products.Update(Product);
                if (result.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                    _context.SaveChanges();
                return result.Entity;
            }
            catch (Exception ex)
            {
                _context.WriteErrorLog(ex);
                return Problem($"An Error Occurred! Error Message: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }

        /// <summary>
        /// DeleteProduct - Endpoint for deleting a Product
        /// </summary>
        /// <param name="id">ID value of the Product</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "DeleteProduct", Description = "Endpoint for updating an existing Product.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var data = _context.Products.FirstOrDefault(x => x.Id == id);
                if (data == null) return NotFound(new { Message = "Invalid Id Value" });
                _context.Products.Remove(data);
                _context.SaveChanges(true);
                return Ok();
            }
            catch (Exception ex)
            {
                _context.WriteErrorLog(ex);
                return Problem($"An Error Occurred! Error Message: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }
    }
}