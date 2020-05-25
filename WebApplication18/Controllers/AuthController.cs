using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplication18.DataAccess;
using WebApplication18.Dtos;
using WebApplication18.Entities;

namespace WebApplication18.Controllers
{
    [Route("auth")]

    public class AuthController : Controller
    {
        //private IAuthRepository _authRepository;
        private IConfiguration _configuration;
        private IUserDal _userDal;
        private IAuthRepositoryUserDal _authRepositoryUser;
        private IAuthRepositoryManagerDal _authRepositoryManager;


        public AuthController( IConfiguration configuration, IUserDal userDal,IAuthRepositoryUserDal authRepositoryUser,IAuthRepositoryManagerDal authRepositoryManager)
        {
            _authRepositoryUser = authRepositoryUser;
            _configuration = configuration;
            _userDal = userDal;
            _authRepositoryManager = authRepositoryManager;
        }
        //public static UsersContext UC = new UsersContext();
        //public static AuthRepository authRepository = new AuthRepository(UC);
        //public static AuthRepositoryManager authRepositoryManager = new AuthRepositoryManager(UC);


        [HttpGet("deneme")]
        public IActionResult denemeFunc(int id)
        {
            id = 2;
            string denemeValue = "Deneme Work  " + id.ToString();
            User user = new User();
            user.NameSurname = denemeValue;
            return Ok(denemeValue);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            
            if (await _authRepositoryUser.UserExists(userForRegisterDto.NameSurname))
            {
                ModelState.AddModelError("UserName", "Username already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userToCreate = new User
            {
                NameSurname = userForRegisterDto.NameSurname,
                Email=userForRegisterDto.Email,
                PhoneNumber = userForRegisterDto.PhoneNumber,


            };
            var createdUser = await _authRepositoryUser.Register(userToCreate, userForRegisterDto.Password);
            return StatusCode(201,createdUser);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto userForLoginDto)
        {

            var user = await _authRepositoryUser.Login(userForLoginDto.NameSurname, userForLoginDto.Password);
            if(user==null)
            {
                return Unauthorized();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.NameSurname)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            ModelToSentToken modelToSentToken = new ModelToSentToken();
            modelToSentToken.tokenString = tokenString;
           
            return Ok(modelToSentToken);
        }




        [HttpPost("user/passwordEdit/{userId}")]
        public IActionResult passwordEdit(int userId, [FromBody]NewPasswordForEditDto newPasswordForEditDto)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (currentUserId == userId)
                {
                    User user = _userDal.GetUserById(userId);
                    var authUser = _authRepositoryUser.Login(user.NameSurname, newPasswordForEditDto.currentPassword);
                    if (authUser == null)
                    {
                        StatusMessages statusMessages = new StatusMessages();
                        statusMessages.message = "Password is not correct!";
                        return Ok(statusMessages);
                    }

                    using (var hmac = new System.Security.Cryptography.HMACSHA512())
                    {
                        user.PasswordHash = hmac.Key;
                        user.PasswordSalt = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newPasswordForEditDto.newPassword)); ///////////////////*******************************
                    }
                    //byte[] passwordHash, passwordSalt;
                    //_authRepository.CreatePasswordHash(newPasswordForEditDto.newPassword, out passwordHash, out passwordSalt);
                    //user.PasswordHash = passwordHash;
                    //user.PasswordSalt = passwordSalt;

                    _userDal.Update(user);

                    return Ok(201);
                }
                else
                {
                    return NotFound(404);
                }
            }
            catch
            {
                return NotFound(404);
            }


        }



        // AFTER THIS LINE IS ABOUT MANAGER ===============================================>>>>>>>>>>>>>>>>>>>








        [HttpPost("manager/register")]
        public async Task<IActionResult> RegisterManager([FromBody]UserForRegisterDto userForRegisterDto)
        {

            if (await _authRepositoryManager.UserExists(userForRegisterDto.NameSurname))
            {
                ModelState.AddModelError("UserName", "Username already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var managerToCreate = new Manager
            {
                NameSurname = userForRegisterDto.NameSurname,
                Email = userForRegisterDto.Email,
                PhoneNumber = userForRegisterDto.PhoneNumber,


            };
            var createdManager = await _authRepositoryManager.Register(managerToCreate, userForRegisterDto.Password);
            return StatusCode(201, createdManager);
        }




        [HttpPost("manager/login")]
        public async Task<IActionResult> LoginManager([FromBody]UserForLoginDto userForLoginDto)
        {

            var manager = await _authRepositoryManager.Login(userForLoginDto.NameSurname, userForLoginDto.Password);
            if (manager == null)
            {
                return Unauthorized();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier,manager.Id.ToString()),
                    new Claim(ClaimTypes.Role,manager.NameSurname)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            ModelToSentToken modelToSentToken = new ModelToSentToken();
            modelToSentToken.tokenString = tokenString;

            return Ok(modelToSentToken);
        }
        
        
    }
    


}