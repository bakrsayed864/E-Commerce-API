using Microsoft.AspNetCore.Mvc;
using Own_Service.DTO;
using Own_Service.Models;
using System.Collections.Generic;

namespace Own_Service.Services
{
    public interface ICustomerRepository
    {
        List<CustomerDTO> getAll();
        CustomerDTO getById(int id);
        int Update(CustomerDTO customerDTO,int id);
        int Delete(int id);
        int Create(RegisterUserDTO registerUserDTO, string id);
    }
}