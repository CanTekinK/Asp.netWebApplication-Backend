using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication18.DataAccess;
using WebApplication18.Dtos;
using WebApplication18.Entities;
using WebApplication18.MiddleWares;

namespace WebApplication18.Controllers
{


    [Route("home")]


    public class UserController : Controller
    {
        private IUserDal _userDal;
        private IMessageDal _messageDal;
        private IUserPhotoDal _userPhotoDal;
        private IMapper _mapper;
        public UserController(IUserDal userDal, IMessageDal messageDal, IUserPhotoDal userPhotoDal, IMapper mapper)
        {
            _userDal = userDal;
            _messageDal = messageDal;
            _userPhotoDal = userPhotoDal;
            _mapper = mapper;
        }

        [HttpGet("users")]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetList()
        {
            //var Users = UC.Users.Include(x => x.UserPhotos).Select(data => new User { Email=data.Email,NameSurname=data.NameSurname,UserPhotos=data.UserPhotos}).ToList();
            // UsersContext UC = new UsersContext();
            // var users3 = UC.Users.Include(x => x.UserPhotos).ToList();

            
            try
            {

                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var contextResult = _userDal.GetUsers();
                var mappingResult = _mapper.Map<List<UserForListDto>>(contextResult);
                UserForListDto userForListDto = new UserForListDto();

                userForListDto = mappingResult.Find(data => data.Id == currentUserId);

                mappingResult.Remove(userForListDto);
                return Ok(mappingResult);





            }
            catch
            {
                return NotFound(404);
            }
            
        }
        //[Authorize(Roles = "9")]
        [HttpGet("users/details/{Id}")]
        public ActionResult GetUser(int Id)
        {
            var currentUserId =  Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(currentUserId!=null)
            {
                var result = _userDal.GetUserById(Id);
                return Ok(result);
            }
            return NotFound(404);
            
            
           


        }
        [HttpGet("messages")]
        public IActionResult GetMessages()
        {
            var result = _messageDal.GetMessages();
            return Ok(result);
        }
        [HttpGet("messages/{Id}")]
        public IActionResult GetMessagesById(int Id)
        {
            var result = _messageDal.GetMessagesById(Id);
            return Ok(result);
        }
        [HttpGet("users/messages/{Id}")]
        public IActionResult GetMessagesByUser(int Id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            
            if (currentUserId==Id)
            {
                var result = _messageDal.GetMessagesByUser(Id);
                var mappingResult = _mapper.Map<List<UserMessagesForListDto>>(result);
                return Ok(mappingResult);
            }
            return BadRequest(401);


        }


        [HttpGet("users/photos/{Id}")]
        public IActionResult GetUserPhotosByUser(int Id)
        {
            var result = _userPhotoDal.GetUserPhotoByUser(Id);
            return Ok(result);
        }

        

      
        [HttpPost("user/add")]
        public IActionResult Add(User user)
        {
            
            try
            {
                _userDal.Add(user);
                return new StatusCodeResult(201);
            }
            catch
            { }
            return BadRequest();
        }
        [HttpPost("user/sendmessage")]
        public IActionResult AddMeesage([FromBody]MessageSendDto message)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            if(currentUserId==message.UserId)
            {
                var currentUser = _userDal.GetUserById(currentUserId);
                if(currentUser.NameSurname==message.SenderName)
                {
                    try
                    {

                        Message addMessage = new Message();
                        
                        addMessage.Content = message.Content;
                        addMessage.MState = message.MState;
                        addMessage.Time = DateTime.Now;
                        addMessage.UserId = message.UserId;
                        if(addMessage.Content==null)
                        {
                            addMessage.Content = "Açıklama yok.";
                        }



                        _messageDal.Add(addMessage);
                        return new StatusCodeResult(201);
                    }
                    catch
                    { }
                    return BadRequest();

                }
                else
                {
                    return Unauthorized();
                }
                

            }
            else
            {
                return NotFound(404);
            }

            
        }


        [HttpPost("photo/add")]
        public IActionResult AddPhoto([FromBody] UserPhoto userPhoto)
        {

            try
            {
              
                
                _userPhotoDal.Add(userPhoto);
         
                return new StatusCodeResult(201);
            }
            catch
            { }
            return BadRequest();
        }



        [HttpPost("users/details/update")]
        public IActionResult Update([FromBody]UserForEditDto userForEditDto)
        {
            /*var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId == userForEditDto.Id)
            {
                
            }*/


            User user = _userDal.GetUserById(userForEditDto.Id);
            if(userForEditDto.NameSurname!="" && userForEditDto.PhoneNumber!="" && userForEditDto.Email!="")
            {
     
                
                user.Email = userForEditDto.Email;
                user.NameSurname = userForEditDto.NameSurname;
                user.PhoneNumber = userForEditDto.PhoneNumber;
         
          
                _userDal.Update(user);
                return new StatusCodeResult(201);
                /*
               byte[] passwordHash, passwordSalt;
               authRepository.CreatePasswordHash(userForEditDto.Password, out passwordHash, out passwordSalt);
               user.PasswordHash = passwordHash;
               user.PasswordSalt = passwordSalt;*/
            }
            else
            {
                return Ok("Empty properties!!!");
            }
            
            //_userDal.Update(user);
            
            /*try
            {
                var currentUserId= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
               if(currentUserId== userForEditDto.Id)
                {
                    User user = new User();
                    UsersContext usersContext = new UsersContext();
                    AuthRepository authRepository = new AuthRepository(usersContext);
                    byte[] passwordHash, passwordSalt;
                    user.Email = userForEditDto.Email;
                    user.NameSurname = userForEditDto.NameSurname;
                    user.PhoneNumber = userForEditDto.PhoneNumber;
                    
                    
                    authRepository.CreatePasswordHash(userForEditDto.password, out passwordHash, out passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    _userDal.Update(user);
                    return new StatusCodeResult(201);
                } 
                else
                {
                    return NotFound(404);
                }
               
            }
            catch
            { }
            return BadRequest();*/
        }
        [HttpDelete("{UserId}")]
        public IActionResult Delete(int UserId)
        {
            try
            {
                _userDal.Delete(new User { Id=UserId});
                return new StatusCodeResult(201);
            }
            catch
            { }
            return BadRequest();
        }
      
       
    }
}
