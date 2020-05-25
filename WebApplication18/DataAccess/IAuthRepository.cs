using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication18.Entities;

namespace WebApplication18.DataAccess
{
    public interface IAuthRepository<T>
    {
        Task<T> Register(T user, string password);
        Task<T> Login(string userName, string password);
        Task<bool> UserExists(string userName);


    }
}
