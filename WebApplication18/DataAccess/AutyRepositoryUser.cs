using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication18.Entities;

namespace WebApplication18.DataAccess
{
    public class AuthRepositoryUser<TContext> : IAuthRepository<User>
        where TContext:DbContext,new()
     
      
    {
        public async Task<User> Register(User user, string password)
        {
            using(var context=new TContext())
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                var adding=context.Entry(user);
                adding.State = EntityState.Added;
                await context.SaveChangesAsync();
                return user;
            }
            
            
            
        }


        public void CreatePasswordHash(string password,out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac=new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); ///////////////////*******************************
            }
        }

        public async Task<User> Login(string userName, string password)
        {
            using (var context = new TContext())
            {
                
                var user =await context.Set<User>().FirstOrDefaultAsync(x => x.NameSurname == userName);

                if (user == null)
                {
                    return null;
                }
                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                {
                    return null;
                }
                return user;

            }
            
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0;i<computedHash.Length;i++)
                {
                    if(computedHash[i]!=passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

       

        

        public async Task<bool> UserExists(string userName)
        {
            using (var context = new TContext())
            {
                if (await context.Set<User>().AnyAsync(x => x.NameSurname == userName))
                {
                    return true;
                }
                return false;

            }
                
        }

        

        

        
    }
}
