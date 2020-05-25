using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication18.Entities;

namespace WebApplication18.Dtos
{
    public class UserForListDto
    {


        public int Id { get; set; }
        public string Email { get; set; }



        public string NameSurname { get; set; }

        public string PhoneNumber { get; set; }
        public string Url { get; set; }


    }
}
