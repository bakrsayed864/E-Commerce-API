//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Routing;
//using Own_Service.DTO;
//using Own_Service.Models;
//using Own_Service.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Own_Service.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmployeeController : ControllerBase
//    {
//        IEmployeeRepository employeeRepository; 
//        public EmployeeController(Entities _context,IEmployeeRepository _employeeRepository)
//        {
//            employeeRepository = _employeeRepository;
//        }
//        [HttpGet]
//        public IActionResult getAllEmployees()
//        {
//            List<Employee> employees=employeeRepository.GetAll();
//            if (employees != null)
//            {
//                EmployeeWithDepartmentName emp=new EmployeeWithDepartmentName();
//                List<EmployeeWithDepartmentName> empDto = new List<EmployeeWithDepartmentName>();
//                foreach (Employee employee in employees)
//                {
//                    emp.phone = employee.phone;
//                    emp.Name = employee.Name;
//                    emp.Address = employee.Address;
//                    emp.Id = employee.Id;
//                    emp.DeptName=employee.Department.Name;
//                    empDto.Add(emp);
//                }
//                return Ok(empDto); //response body(Json)
//            }
//            return BadRequest("no employees found");
//        }
//        [HttpGet("{id:int}",Name ="GetoneEmployee")]
//        //[HttpGet]
//        //[Route("{id}")] //api/Department/id
//        public IActionResult getById(int id)
//        {
//            Employee employee = employeeRepository.getById(id);
//            if(employee!=null)
//            {
//                EmployeeWithDepartmentName empDto = new EmployeeWithDepartmentName();
//                empDto.phone = employee.phone;
//                empDto.Name = employee.Name;
//                empDto.Address = employee.Address;
//                empDto.DeptName = employee.Department.Name;
//                empDto.Id= employee.Id;
//                return Ok(empDto);
//            } 
//            return BadRequest("employee not found");
//        }
//        [HttpGet("{name:alpha}")]
//        public IActionResult getByName(string name)
//        {
//            Employee employee = employeeRepository.getByname(name);
//            if(employee!=null)
//            {
//                return Ok(employee);
//            }
//            return BadRequest(ModelState);
//            //return BadRequest("employee not found");
//        }
//        [HttpPost]
//        public IActionResult AddEmployee(Employee employee)
//        {
//            if (ModelState.IsValid)
//            {
//                employeeRepository.Create(employee);
//                //ok can be returned but as a standard Created(status code 201) is used when new resource added
//                //return Created("http://localhost:44371/api/Employee/"+employee.Id,employee);
//                //to get the link of action even you are in local host or hosting in the server
//                string link = Url.Link("GetoneEmployee", new { id=employee.Id });
//                return Created(link, employee);
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpDelete("{id:int}")]
//        public IActionResult DeleteEmployee(int id)
//        {
//            Employee employee=employeeRepository.getById(id);
//            if(employee!=null)
//            {
//                try
//                {
//                    employeeRepository.Delete(id);
//                    return StatusCode(204, "employee deleted");
//                }
//                catch(Exception ex) { 
//                return BadRequest(ex.Message);
//                }
//            }
//            return BadRequest(ModelState);
//        }

//        [HttpPut("{id:int}")]
//        public IActionResult UpdateEmployee([FromRoute]int id,[FromBody]Employee employee)
//        {
//            if(ModelState.IsValid)
//            {
//                int changes=employeeRepository.Update(id, employee);
//                if(changes>0)
//                {
//                    return StatusCode(204, "data saved");//204 means no content for return
//                }
//                return BadRequest(ModelState);
//            }
//            return BadRequest(ModelState);
//        }
//    }
//}
