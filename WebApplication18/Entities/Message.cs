using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication18.Entities
{
    public class Message: IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public int MState { get; set; }
        public User user { get; set; }

    }
}
