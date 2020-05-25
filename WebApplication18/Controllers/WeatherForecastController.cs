using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication18.DataAccess;
using WebApplication18.Entities;

namespace WebApplication18.Controllers
{
  
    [Route("saving")]
   
    public class WeatherForecastController : Controller
    {
       // private UsersContext _context;
        private IUserDal _userDal;
        public WeatherForecastController(IUserDal userDal)
        {
            //_context = context;
            _userDal = userDal;

        }
        [HttpPost]
        public IActionResult Saving()
        {

            UsersContext _context = new UsersContext();
            User user = new User();
            user = _userDal.GetUserById(9);
            UserPhoto userPhoto = new UserPhoto();
            userPhoto.IsMain = false;
            userPhoto.Url = "https://i.kym-cdn.com/entries/icons/facebook/000/009/803/spongebob-squarepants-patrick-spongebob-patrick-star-background-225039.jpg";
            user.UserPhotos.Add(userPhoto);
            userPhoto.user = user;
            
            if(_context.SaveChanges()>0)
            {
                return Ok("Islem tamam");
            }
            return Ok("basarisiz");
            
            
        }
       
        
    }
}
