using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Own_Service.DTO;
using Own_Service.Services;
using System.Security.Claims;

namespace Own_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewRepository _productReviewRepository;

        public ProductReviewController(IProductReviewRepository productReviewRepository)
        {
            this._productReviewRepository = productReviewRepository;
        }

        [HttpGet("{productId}")]
        public IActionResult getProductReviews(int productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var reviews = _productReviewRepository.GetAll(productId, userId);
            if(reviews == null)
                return NotFound("");
            return Ok(reviews);
        }
        [HttpGet]
        public IActionResult getCustomerReviews()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var reviews = _productReviewRepository.GetAll(userId);
            if (reviews == null)
                return NotFound();
            return Ok(reviews);
        }

        [HttpPost("{productId}")]
        public IActionResult AddReview(ProductReviewDTO productReviewDTO,int productId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            int changes = _productReviewRepository.AddReview(productReviewDTO, productId, userId);  //0 notfound // -1 review updated // -2 customer not found(Unauthorized)
            return changes switch
            {
                -2 => Unauthorized(),
                0 => NotFound(),
                _ => Ok(),
            };
           
        }

        [HttpDelete("/removeMyReview")]
        public IActionResult RemoveCustomerReview([FromHeader] int reviewId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
                return Unauthorized();
            int changes = _productReviewRepository.DeleteReview(reviewId, userId);
            return changes switch
            {
                0 => Unauthorized(),
                -1 => NotFound("no product review found for this customer"),
                _ => NoContent(),
            };
        }

        [HttpDelete("/removeReview")]
        public IActionResult Remove([FromHeader] int reviewId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            int changes = _productReviewRepository.DeleteReview(reviewId);
            return changes switch
            {
                0 => Unauthorized(),
                -1 => NotFound("no product review found for this customer"),
                _ => NoContent(),
            };
        }

    }
}
