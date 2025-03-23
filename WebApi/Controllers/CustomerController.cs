using Entities.DTOs;
using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.Concrete;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("getlistpagedcustomer")]
        public IActionResult GetListPagedCustomer([FromQuery] CustomerFilterDto customerFilterDto, [FromQuery] PageRequestDto pageRequestDto)
        {
            var result = _customerService.GetListPagedCustomer(customerFilterDto, pageRequestDto);

            return Ok(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Customer customer)
        {

            _customerService.Add(customer);
            return Ok(new { Message = "Customer successfully added." });

        }

        [HttpPost("update")]
        public IActionResult Update(Customer customer)
        {

            _customerService.Update(customer);
            return Ok(new { Message = "Customer successfully updated." });

        }

        [HttpPost("delete")]
        public IActionResult Delete(Customer customer)
        {

            _customerService.Delete(customer);
            return Ok(new { Message = "Customer successfully deleted." });

        }
    }
}
