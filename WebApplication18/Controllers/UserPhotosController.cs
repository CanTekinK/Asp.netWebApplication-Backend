using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplication18.DataAccess;
using WebApplication18.Dtos;
using WebApplication18.Helpers;
using System.Security.Claims;
using CloudinaryDotNet.Actions;
using WebApplication18.Entities;

namespace WebApplication18.Controllers
{
   
    [Route("userphoto/{userId}/photos")]
  
    public class UserPhotosController : Controller
    {
        private Cloudinary _cloudinary;
        private IUserPhotoDal _userPhotoDal;
        private IUserDal _userDal;
        private IMapper _mapper;
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        //private UsersContext _context;

        
        public UserPhotosController( IUserDal userDal,IUserPhotoDal userPhotoDal, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            //_context = context;
            _userPhotoDal = userPhotoDal;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            _userDal = userDal;
            

            Account account = new Account(_cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(account);
        }
        [HttpPost]
        public ActionResult AddUserPhoto(int userId,[FromForm]UserPhotoForCreationDto userPhotoForCreationDto)
        {
            UsersContext _context = new UsersContext();
            var user = _userDal.GetUserById(userId);
            if(user==null)
            {
                return BadRequest("Cloud not find the user.");
            }
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId!=user.Id)
            {
                return Unauthorized();
            }
            var file = userPhotoForCreationDto.File;
            var uploadResult = new ImageUploadResult();
            if(file.Length>0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream)
                    };
                    
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            userPhotoForCreationDto.Url = uploadResult.Uri.ToString();
            userPhotoForCreationDto.PublicId = uploadResult.PublicId;
            var photo = _mapper.Map<UserPhoto>(userPhotoForCreationDto);
            photo.user = user;
            if (!user.UserPhotos.Any(p=>p.IsMain))
            {
                photo.IsMain = true;
            }
            user.UserPhotos.Add(photo);
            photo.UserId = user.Id;
            _userPhotoDal.Add(photo);
            _context.SaveChanges();

            /*if (_context.SaveChanges()>0)
            {
                var photoToReturn = _mapper.Map<UserPhotoForReturnDto>(photo);
                return CreatedAtRoute("GetUserPhoto", new { Id = photo.Id }, photoToReturn);
            }*/
            return Ok(201);
        }
        [HttpGet("{Id}",Name="GetUserPhoto")]
        public ActionResult GetPhoto(int Id)
        {
            var photoFromDB = _userPhotoDal.GetUserPhoto(Id);
            var photo = _mapper.Map<UserPhotoForReturnDto>(photoFromDB);
            return Ok(photo);
        }

    }
}