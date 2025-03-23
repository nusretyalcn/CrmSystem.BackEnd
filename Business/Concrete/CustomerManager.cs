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

                var paginatedResult = query.ToPaginate(pageRequestDto.PageIndex, pageRequestDto.PageSize);

                return new SuccessDataResult<Paginate<Customer>>(paginatedResult, "Müşteriler Listelendi");
            }
            catch (Exception)
            {

                return new ErrorDataResult<Paginate<Customer>>(null, "Müşteriler Listelendi");
            }
        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult Add(Customer customer)
        {
            try
            {
                customer.RegistrationDate = DateTime.UtcNow;
                _customerDal.Add(customer);
                return new SuccessResult("Müşteri Eklendi");
            }
            catch (Exception ex)
            {

                return new ErrorResult("Müşteri Eklenemedi");
            }
        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult Delete(Customer customer)
        {
            try
            {
                _customerDal.Delete(customer);
                return new SuccessResult("Müşteri silindi");
            }
            catch (Exception ex)
            {

                return new ErrorResult("Müşteri silinemedi");
            }
        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult Update(Customer customer)
        {
            try
            {
                _customerDal.Update(customer);
                return new SuccessResult("Müşteri güncellendi");
            }
            catch (Exception ex)
            {

                return new ErrorResult("Müşteri güncellenemedi");
            }
        }
    }
}
