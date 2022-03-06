using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dto.User;
using UserService.Models;

namespace UserService.Profiles
{
    public class UserProfiles: Profile
    {
        public UserProfiles()
        {
            // Source -> Targer

            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserReadDto>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserExams, UserExamDto>();

        }
    }
}
