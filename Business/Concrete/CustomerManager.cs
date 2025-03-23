using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Pagging;

namespace Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public Paginate<Customer> GetListPagedCustomer(CustomerFilterDto customerFilterDto, PageRequestDto pageRequestDto)
        {

            Expression<Func<Customer, bool>> predicate = p => true; // Başlangıçta tüm veriyi getir

            if (!string.IsNullOrEmpty(customerFilterDto.Name))// Name filtresini kontrol ediyor
                predicate = predicate == null //filtre oluşturulmamışsa oluşturuluyor
                    ? c => c.FirstName.Contains(customerFilterDto.Name)
                    : predicate.And(c => c.FirstName.Contains(customerFilterDto.Name));//filtre oluşturulmuşsa mevcut predicate'e yeni koşul ekliyor

            if (!string.IsNullOrEmpty(customerFilterDto.Email))
                predicate = predicate == null
                    ? c => c.Email.Contains(customerFilterDto.Email)
                    : predicate.And(c => c.Email.Contains(customerFilterDto.Email));

            if (!string.IsNullOrEmpty(customerFilterDto.Region))
                predicate = predicate == null
                    ? c => c.Region.Contains(customerFilterDto.Region)
                    : predicate.And(c => c.Region.Contains(customerFilterDto.Region));

            if (customerFilterDto.StartDate.HasValue)
                predicate = predicate == null
                    ? c => c.RegistrationDate >= customerFilterDto.StartDate.Value
                    : predicate.And(c => c.RegistrationDate >= customerFilterDto.StartDate.Value);

            if (customerFilterDto.EndDate.HasValue)
                predicate = predicate == null
                    ? c => c.RegistrationDate <= customerFilterDto.EndDate.Value
                    : predicate.And(c => c.RegistrationDate <= customerFilterDto.EndDate.Value);



            IQueryable<Customer> query = _customerDal.GetList(
                predicate: predicate,
                orderBy: q => q.OrderByDescending(c => c.Id),
                enableTracking: false);

            return query.ToPaginate(pageRequestDto.PageIndex, pageRequestDto.PageSize);

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
