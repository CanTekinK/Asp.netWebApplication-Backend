using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication18.Entities;

namespace WebApplication18.DataAccess
{
    public class AuthRepositoryManager<TContext> : IAuthRepository<Manager>
         where TContext : DbContext, new()
    {
       
       
        public async Task<bool> UserExists(string userName)
        {
            using (var context = new TContext())
            {
                if (await context.Set<Manager>().AnyAsync(x => x.NameSurname == userName))
                {
                    return true;
                }
                return false;

            }
                
        }
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); ///////////////////*******************************
            }
        }

        public async Task<Manager> Register(Manager manager, string password)
        {
            using (var context = new TContext())
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                manager.PasswordHash = passwordHash;
                manager.PasswordSalt = passwordSalt;
                var adding = context.Entry(manager);
                adding.State = EntityState.Added;
                await context.SaveChangesAsync();
                return manager;

            }
            
        }

        public async Task<Manager> Login(string managerName, string password)
        {
            using (var context = new TContext())
            {
                var manager = await context.Set<Manager>().FirstOrDefaultAsync(x => x.NameSurname == managerName);
                if (manager == null)
                {
                    return null;
                }
                if (!VerifyPasswordHash(password, manager.PasswordHash, manager.PasswordSalt))
                {
                    return null;
                }
                return manager;

            }
                
        }

       
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
