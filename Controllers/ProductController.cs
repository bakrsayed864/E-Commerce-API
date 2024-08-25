using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Own_Service.DTO;
using Own_Service.Models;
using Own_Service.Services;

namespace Own_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        [HttpPost]
        public IActionResult Create(ProductDTO productDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Product createdProduct = _productRepository.Create(productDto);
                if (createdProduct == null)
                    return BadRequest("product not saved correctly");
                var url = Url.Link("getOneProduct", new { createdProduct.Id });
                return Created(url, createdProduct);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult getAll()
        {
            var products=_productRepository.getAll();
            if (products == null || products.Count==0)
                return NotFound();
            return Ok(products);
        }

        [HttpGet("{id:int}",Name ="getOneProduct")]
        public IActionResult getOne(int id)
        {
            try
            {
                var product = _productRepository.GetById(id);
                if (product == null)
                    return NotFound();
                return Ok(product);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("category/{categoryId}",Name ="getCategoryProducts")]
        public IActionResult getByCategory(int categoryId)
        {
            var products = _productRepository.GetByCategory(categoryId);
            if (products == null || products.Count == 0)
                return NotFound();
            return Ok(products);
        }

        [HttpGet("productName/{name:alpha}")]
        public IActionResult getByName(string name)
        {
            var products = _productRepository.GetByName(name);
            if (products == null || products.Count == 0)
                return NotFound();
            return Ok(products);
        }

        [HttpPut("{id}")]
        public IActionResult Update(ProductDTO productDto,int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                int changes = _productRepository.Update(productDto, id);
                if (changes == 0)
                    return BadRequest("update product failed to complete");
                return StatusCode(204, "data updated succesfully");
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                int changes = _productRepository.Delete(id);
                if (changes == 0)
                    return NotFound();
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}