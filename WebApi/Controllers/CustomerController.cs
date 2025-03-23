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
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
            
        }

        [HttpPost("add")]
        public IActionResult Add(Customer customer)
        {

            var result = _customerService.Add(customer);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpPost("update")]
        public IActionResult Update(Customer customer)
        {

            var result = _customerService.Update(customer);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpPost("delete")]
        public IActionResult Delete(Customer customer)
        {

            var result = _customerService.Delete(customer);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
    }
}
