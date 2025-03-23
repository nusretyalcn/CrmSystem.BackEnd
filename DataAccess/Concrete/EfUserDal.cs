using Core.DataAccess;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{

    public class EfUserDal : EfEntityRepositoryBase<User, EfDbContext>, IUserDal
    {
        public EfUserDal(EfDbContext context) : base(context)
        {
        }

        public List<OperationClaim> GetClaims(User user)
        {

                var result = from operationClaim in _context.OperationClaims
                             join userOperationClaim in _context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return result.ToList();

        }
    }
}
