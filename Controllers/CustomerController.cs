using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Own_Service.DTO;
using Own_Service.Models;
using Own_Service.Services;
using System.Collections.Generic;

namespace Own_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult getAllCustomers()
        {
            List<CustomerDTO> customers = _customerRepository.getAll();
            if (customers == null)
            {
                return BadRequest("no users to show");
            }
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public IActionResult geById(int id)
        {
            var customer=_customerRepository.getById(id);
            if(customer == null)
            {
                return BadRequest("there is no user found with specified id");
            }
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public IActionResult Update(CustomerDTO customerDTO,int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            int changes=_customerRepository.Update(customerDTO,id);
            if (changes == 0)
                return BadRequest("ther is no user with specified if");

            return StatusCode(204,"data updated successfully");
        }
        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            int changes= _customerRepository.Delete(id);
            if (changes == 0)
                return BadRequest("there is no user with specified id");
            return Ok();
        }

    }
}
