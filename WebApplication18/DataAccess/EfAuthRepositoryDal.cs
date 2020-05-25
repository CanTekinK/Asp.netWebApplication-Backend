using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication18.Entities;

namespace WebApplication18.DataAccess
{
    public class EfAuthRepositoryUserDal : AuthRepositoryUser<UsersContext>,IAuthRepositoryUserDal
    {
        
    }
    public class EfAuthRepositoryManagerDal :AuthRepositoryManager<UsersContext>,IAuthRepositoryManagerDal
    {

    }
}
