
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICustomerService
    {
        List<Customer> GetListPagedCustomer(CustomerFilterDto customerFilterDto);
        void Add(CustomerDto customerDto);
        void Update(Customer customer);
        void Delete(Customer customer);
    }
}
