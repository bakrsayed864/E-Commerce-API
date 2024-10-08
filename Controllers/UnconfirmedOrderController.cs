﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
            if(!ModelState.IsValid) 
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

        [HttpGet("/getUserUnconfirmedOrders")]
        public IActionResult getUserUncofirmedOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<UnconfirmedOrderWithProductNameDTO> list = _unconfirmedOrderRepo.getAll(userId);
            if(list == null)
                return NotFound("there is no unconfirmed orders for this user");
            return Ok(list);
        }
        [HttpGet("/getallUsersUnconfirmedOrders")]
        public IActionResult getAll()
        {
            List<UnconfirmedOrderWithProductNameDTO> list = _unconfirmedOrderRepo.getAll();
            if (list == null)
                return NotFound("there is no unconfirmed orders");
            return Ok(list);
        }
        [HttpGet("/getSpecific",Name ="getUnconfirmdeOrderById")]
        public IActionResult getById([FromHeader]int id)
        {
            var unconfOrder=_unconfirmedOrderRepo.getById(id);
            if (unconfOrder == null)
                return NotFound();
            return Ok(unconfOrder);
        }

        [HttpGet("/getTotalPrice")]
        public IActionResult getTotaPrice()
        {
            string userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            double price=_unconfirmedOrderRepo.getTotalPrice(userId);
            return Ok(price);
        }

        [HttpPut]
        public IActionResult Edit([FromHeader] int newQuantity, [FromHeader] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            UnconfirmedOrderDTO result = _unconfirmedOrderRepo.Edite(newQuantity, id);
            if (result == null)
                return BadRequest("product not found or quanttiy exceeded the availabe");
            return Ok(result);
        }
        [HttpDelete]
        public IActionResult DeleteUserOrder([FromHeader] int orderId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int changes;
            if (User.IsInRole("Admin"))
            {
                changes = _unconfirmedOrderRepo.Delete(orderId);
            }
            else
            {
                changes = _unconfirmedOrderRepo.Delete(userId, orderId);
            }
            if(changes == 0)
                return NotFound();
            return NoContent();
        }
       
    }
}
