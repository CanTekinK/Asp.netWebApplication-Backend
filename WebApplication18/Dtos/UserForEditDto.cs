﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication18.Dtos
{
    public class UserForEditDto
    {
       


        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    

        public string NameSurname { get; set; }

        public string PhoneNumber { get; set; }
   
    }
}
