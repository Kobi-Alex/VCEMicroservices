using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Profiles
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            // Source -> Targer

            CreateMap<User, UserReadDto>();
            CreateMap<UserReadDto, User> ();
            CreateMap<UserUpdateDto, User> ();
        }
    }
}
