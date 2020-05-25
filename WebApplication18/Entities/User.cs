using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication18.Entities
{
    public class User:IEntity
    {
        /*public User()
        {
            UserPhotos = new List<UserPhoto>();
            Messages = new List<Message>();
        }*/
            

        public int Id { get; set; }
        public string Email { get; set; }
       
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string NameSurname { get; set; }

        public string PhoneNumber { get; set; }
        public List<UserPhoto> UserPhotos { get; set; }
        public List<Message> Messages { get; set; }
    }
}
