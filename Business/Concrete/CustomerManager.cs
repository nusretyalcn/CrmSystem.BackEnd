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
using Core.Utilities.Validation;
using Business.ValidationRules;

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

            try
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
            catch (Exception ex)
            {
                throw new Exception("Customers could not be listed.", ex);
            }
        }


        public void Add(CustomerDto customerDto)
        {
            try
            {             
                var customer = new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Region = customerDto.Region,
                    Email = customerDto.Email,
                    RegistrationDate = DateTime.UtcNow
                };

                ValidationTool.Validate(new CustomerValidator(), customer);

                _customerDal.Add(customer);
            }
            catch (Exception ex)
            {

                throw new Exception("Failed to add customer:", ex);
            }

        }

        public void Delete(Customer customer)
        {
            try
            {
                ValidationTool.Validate(new CustomerValidator(), customer);
                _customerDal.Delete(customer);
            }
            catch (Exception ex)
            {

                throw new Exception("Failed to delete customer:", ex);
            }

        }

        public void Update(Customer customer)
        {
            try
            {
                ValidationTool.Validate(new CustomerValidator(), customer);
                _customerDal.Update(customer);
            }
            catch (Exception ex)
            {

                throw new Exception("Failed to update customer:", ex);
            }

        }
    }
}
