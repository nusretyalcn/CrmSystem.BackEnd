
using Core.Utilities.Pagging;
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
        Paginate<Customer> GetListPagedCustomer(CustomerFilterDto customerFilterDto, PageRequestDto pageRequestDto);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
    }
}
