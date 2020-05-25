using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication18.Dtos
{
    public class UserPhotoForCreationDto
    {
        public UserPhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
 
  
        public IFormFile File { get; set; }
        public string Description { get; set; }
     
        public DateTime DateAdded { get; set; }
        public string Url { get; set; }



        public string PublicId { get; set; }
      
    }
}
