using System;

namespace Applicant.API.Application.Services.Interfaces
{
    public interface IServiceManager
    {
        IAccessCodeService AccessCodeService { get; }
        IUserService UserService { get; }
    }
}