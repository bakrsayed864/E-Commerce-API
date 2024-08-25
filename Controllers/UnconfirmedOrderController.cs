using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Own_Service.DTO;
using Own_Service.Models;
using Own_Service.Services;
using System.Collections.Generic;
using System.Security.Claims;

namespace Own_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnconfirmedOrderController : ControllerBase
    {
        private readonly IUnconfirmedOrderRepository _unconfirmedOrderRepo;

        public UnconfirmedOrderController(IUnconfirmedOrderRepository unconfirmedOrderRepo)
        {
            this._unconfirmedOrderRepo = unconfirmedOrderRepo;
        }
        [HttpPost]
        public IActionResult Create(UnconfirmedOrderDTO unconfirmedOrderDTO)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            else if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            int changes = _unconfirmedOrderRepo.Add(unconfirmedOrderDTO,userId);
            string url = Url.Link("getUnconfirmdeOrderById", new { id=changes });
            return changes switch
            {
                0 | -3 => BadRequest(ModelState),
                -1=>NotFound("product not found"),
                -2 => BadRequest("required quantity exceeded the availabe quantity"),
                _ => Created(url,unconfirmedOrderDTO),
            };
        }
        [HttpGet]
        public IActionResult getAll()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
                return Unauthorized();
            List<UnconfirmedOrderWithProductNameDTO> list = _unconfirmedOrderRepo.getAll(userId);
            return Ok(list);
        }
        [HttpGet("{id}",Name ="getUnconfirmdeOrderById")]
        public IActionResult getById(int id)
        {
            var unconfOrder=_unconfirmedOrderRepo.getById(id);
            if (unconfOrder == null)
                return NotFound();
            return Ok(unconfOrder);
        }
    }
}
