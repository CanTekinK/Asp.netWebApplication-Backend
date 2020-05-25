using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication18.DataAccess;
using WebApplication18.Dtos;
using WebApplication18.Entities;
using WebApplication18.Helpers;

namespace WebApplication18.Controllers
{

    [Route("manager")]
    [ApiController]
    public class ManagerController : Controller
    {
        private IMessageDal _messageDal;
        private IMapper _mapper;
        private IManagerDal _managerDal;
        public ManagerController(IMessageDal messageDal,IMapper mapper,IManagerDal managaerDal)
        {
            _messageDal = messageDal;
            _mapper = mapper;
            _managerDal = managaerDal;


        }
        [HttpGet("messages/{Id}")]
        public IActionResult GetMessages(int Id)
        {
            int currentManagerId= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            if (Id==currentManagerId)
            {
                var result = _messageDal.GetMessages();
                var mappingResult = _mapper.Map<List<UserMessagesForListDto>>(result);

                return Ok(mappingResult);
            }
            return NotFound(404);
            
        }
        [HttpPost("messages/{managerId}/setstate/{messageId}")]
        public IActionResult SetMessageState(int managerId, int messageId,MessageSetStateDto messageSetStateDto)
        {
            int currentManagerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentManagerId==managerId)
            {
                Message message = _messageDal.GetMessageById(messageId);
                MailSender mail = new MailSender();
                Manager manager=_managerDal.GetManagerById(managerId);

                if (messageSetStateDto.mState==true)
                {
                    message.MState = 2;
                    _messageDal.Update(message);
                    mail.mailSender("İşleminiz onaylandı.", messageSetStateDto.email, manager.Email, "Example Pssword");

                }
                else if (messageSetStateDto.mState == false)
                {
                    message.MState = 1;
                    _messageDal.Update(message);
                    mail.mailSender("İşleminiz onaylanmadı.", messageSetStateDto.email, manager.Email, "Example Password");
                }
                else
                {
                    return BadRequest();
                }
                
                

                
                return Ok(201);
            }
            else
            {
                return NotFound(404);
            }
        }
    }
}