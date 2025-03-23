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
using Core.Aspects;
using Core.Utilities.Results;

namespace Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public IDataResult<Paginate<Customer>> GetListPagedCustomer(CustomerFilterDto customerFilterDto, PageRequestDto pageRequestDto)
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

            var paginatedResult = query.ToPaginate(pageRequestDto.PageIndex, pageRequestDto.PageSize);

            return new SuccessDataResult<Paginate<Customer>>(paginatedResult, "Müşteriler Listelendi");

        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult Add(Customer customer)
        {

            customer.RegistrationDate = DateTime.UtcNow;
            _customerDal.Add(customer);
            return new SuccessResult("Müşteri Eklendi");

        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult Delete(Customer customer)
        {

            _customerDal.Delete(customer);
            return new SuccessResult("Müşteri silindi");
        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult Update(Customer customer)
        {

            _customerDal.Update(customer);
            return new SuccessResult("Müşteri güncellendi");

        }
    }
}
