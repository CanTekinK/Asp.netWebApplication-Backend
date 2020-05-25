using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication18.Entities;

namespace WebApplication18.DataAccess
{
    public interface IAuthRepositoryUserDal:IAuthRepository<User>{ }
    public interface IAuthRepositoryManagerDal:IAuthRepository<Manager>{}
}
