using Microsoft.AspNetCore.Mvc;
using Own_Service.DTO;
using Own_Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace Own_Service.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CommerceDbContext _commerceDbContext;
        public CustomerRepository(CommerceDbContext commerceDbContext)
        {
            _commerceDbContext = commerceDbContext;
        }
        public List<CustomerDTO> getAll()
        {
            var list= _commerceDbContext.Customers.ToList();
            if (list == null)
                return null;
            //DTO map
            List<CustomerDTO> ret = new List<CustomerDTO>();
            foreach (var customer in list)
            {
                //map to DTO and Add
                ret.Add(mapToDto(customer));
            }
            return ret;
        }
        public CustomerDTO getById(int id)
        {
            var customer = _commerceDbContext.Customers.Find(id);
            //map to DTO
            if(customer == null)
                return null;
            return mapToDto(customer);
        }
        //used when registering new user
        public int Create(RegisterUserDTO registerUserDTO, string id)
        {
            _commerceDbContext.Customers.Add(mapToCustomer(registerUserDTO,id));
            return _commerceDbContext.SaveChanges();
        }
        public int Update(CustomerDTO customerDTO,int id)
        {
            var customer = _commerceDbContext.Customers.Find(id);
            if (customer == null)
                return 0;
            customer.BirthDate=customerDTO.BirthDate;
            customer.Name = customerDTO.Name;
            customer.Gender = customerDTO.Gender;
            customer.Job = customerDTO.Job;
            _commerceDbContext.Customers.Update(customer);
            return _commerceDbContext.SaveChanges();
        }
        public int Delete(int id)
        {
            var customer = _commerceDbContext.Customers.Find(id);
            if(customer==null)
                return 0;
            _commerceDbContext.Customers.Remove(customer);
            return _commerceDbContext.SaveChanges(); 
        }
        private CustomerDTO mapToDto(Customer customer)
        {
            CustomerDTO customerDTO = new CustomerDTO();
            customerDTO.Name = customer.Name;
            customerDTO.Gender = customer.Gender;
            customerDTO.Job = customer.Job;
            customerDTO.BirthDate = customer.BirthDate;
            customerDTO.Id = customer.Id;
            return customerDTO;
        }
        private Customer mapToCustomer(RegisterUserDTO registerUserDTO,string id)
        {
            Customer customer = new Customer();
            customer.BirthDate = registerUserDTO.BirthDate;
            customer.UserId = id;
            customer.Gender = registerUserDTO.Gender;
            customer.Job = registerUserDTO.Job;
            customer.Name = registerUserDTO.Name;
            return customer;
        }
    }
}
