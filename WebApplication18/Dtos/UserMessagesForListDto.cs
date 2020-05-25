using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication18.Dtos
{
    public class UserMessagesForListDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public int MState { get; set; }
        public string Email { get; set; }
        public string NameSurname { get; set; }
        public string PhoneNumber { get; set; }
    }
}
