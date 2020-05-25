using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication18.Entities;


namespace WebApplication18.DataAccess
{
    public class EfUserDal : EfEntityRepositoryBase<User, UsersContext>, IUserDal
    {
      
        
        public User GetUserById(int Id)
        {
            UsersContext context = new UsersContext();
            var user = context.Users.Include(data =>
                data.UserPhotos
            ).Include(
                data => data.Messages
                ).Where(data => data.Id == Id).FirstOrDefault();
            return user;

        }
        

        public List<User> GetUsers()
        {
            UsersContext context = new UsersContext();
            var Users = context.Users.Include(data => data.UserPhotos).ToList();
            return Users;
        }
    }
    public class EfMessageDal : EfEntityRepositoryBase<Message, UsersContext>, IMessageDal
    {
        public List<Message> GetMessages()
        {
            UsersContext context = new UsersContext();
            var Messages = context.Message.Include(data => data.user).ToList();
            return Messages;
        }

        public List<Message> GetMessagesById(int Id)
        {
            UsersContext context = new UsersContext();
            var Messages = context.Message.Where(data => data.UserId == Id).ToList();
            return Messages;
        }

        public List<Message> GetMessagesByUser(int Id)
        {
            UsersContext context = new UsersContext();
            var UserMessages = context.Message.Include(data =>data.user).Where(data => data.UserId == Id).ToList();
            return UserMessages;
        }
        

        public Message GetMessageById(int messageId)
        {
            UsersContext context = new UsersContext();
            var UserMessage = context.Message.FirstOrDefault(data => data.Id == messageId);
            return UserMessage;
        }
    }
    public class EfUserPhotoDal : EfEntityRepositoryBase<UserPhoto, UsersContext>, IUserPhotoDal
    {
        public UserPhoto GetUserPhoto(int Id)
        {
            UsersContext context = new UsersContext();
            var UserPhoto = context.UserPhotos.Where(data => data.Id == Id).FirstOrDefault();
            return UserPhoto;
        }

        public List<UserPhoto> GetUserPhotoByUser(int Id)
        {
            UsersContext context = new UsersContext();
            var UserPhotos = context.UserPhotos.Where(data => data.UserId == Id).ToList();
            return UserPhotos;
        }
    }
    public class EfManagerDal : EfEntityRepositoryBase<Manager, UsersContext>, IManagerDal
    {
        public Manager GetManagerById(int managerId)
        {
            UsersContext context = new UsersContext();
            var manager = context.Managers.Where(data => data.Id == managerId).FirstOrDefault();
            return manager;
        }

        public List<Manager> GetManagers()
        {
            UsersContext context = new UsersContext();
            var managers= context.Managers.ToList();
            return managers;
        }
    }


}
