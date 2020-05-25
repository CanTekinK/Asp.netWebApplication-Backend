using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication18.Entities;

namespace WebApplication18.DataAccess
{
    public class UsersContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=SirketDb;Trusted_Connection=true";
            optionsBuilder.UseSqlServer(connection);
        }
        public DbSet<Message> Message { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }
        public DbSet<Manager> Managers{ get; set; }
      
       
       
    }
}
