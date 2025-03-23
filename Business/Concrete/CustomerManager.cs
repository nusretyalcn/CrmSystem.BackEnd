using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public List<Customer> GetListPagedCustomer()
        {
            var result = _customerDal.GetList(orderBy:p=>p.OrderByDescending(c=>c.Id),enableTracking:false);
            return result.ToList();
        }


        public void Add(CustomerDto customerDto)
        {
            var customer = new Customer
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Region = customerDto.Region,
                Email = customerDto.Email,
                RegistrationDate = DateTime.UtcNow
            };

            _customerDal.Add(customer);
        }

        public void Delete(Customer customer)
        {
            _customerDal.Delete(customer);
        }

        public void Update(Customer customer)
        {
            _customerDal.Update(customer);
        }
    }
}
