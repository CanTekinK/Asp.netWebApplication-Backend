using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication18.Entities
{
    public class UserPhoto:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }
        public DateTime DateAdded { get; set; }
        public string Url { get; set; }
       
       
     
        public string PublicId { get; set; }
        public User user { get; set; }
    }
}
