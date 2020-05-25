using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication18.Dtos;
using WebApplication18.Entities;

namespace WebApplication18.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>().ForMember(dest => dest.Url, opt =>
            {
                opt.MapFrom(src => src.UserPhotos.FirstOrDefault(p => p.IsMain).Url);
            });
            CreateMap<UserPhotoForCreationDto,UserPhoto >();
            CreateMap<UserPhotoForReturnDto, UserPhoto>();
            CreateMap<Message, UserMessagesForListDto>().ForMember(dest => dest.NameSurname, opt =>
                {
                    opt.MapFrom(src => src.user.NameSurname);
                }).
               ForMember(dest2 => dest2.PhoneNumber, opt2 =>
               {
                   opt2.MapFrom(src2 => src2.user.PhoneNumber);
               }).
               ForMember(dest3 => dest3.Email, opt3 =>
               {
                   opt3.MapFrom(src3 => src3.user.Email);
               });


        }
    }
}
