using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication18.Entities;


namespace WebApplication18.DataAccess
{
    public interface IUserDal:IEntityRepository<User>
    {
        List<User> GetUsers();
        User GetUserById(int Id);
    }
    public interface IMessageDal: IEntityRepository<Message>
    {
        List<Message> GetMessages();
        List<Message> GetMessagesById(int Id);
        List<Message> GetMessagesByUser(int Id);
        Message GetMessageById(int messageId);
    }
    public interface IUserPhotoDal : IEntityRepository<UserPhoto>
    {
        List<UserPhoto> GetUserPhotoByUser(int Id);
        UserPhoto GetUserPhoto(int Id);
    }
    public interface IManagerDal:IEntityRepository<Manager>
    {
        List<Manager> GetManagers();
        Manager GetManagerById(int managerId);
    }


}
