using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Own_Service.Services;
using System.Security.Claims;

namespace Own_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }
        [HttpGet]
        public IActionResult ConfirmOrder(string Address)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int changes = _orderRepository.ConfirmOrder(userId,Address);
            if(changes == 0)
                return BadRequest($"quantities not available or product is not still exist");
            return Ok("order completed successfully");
        }
    }
}
