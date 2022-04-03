using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Applicant.API.Application.Configurations;
using Applicant.API.Application.Contracts.Dtos.AuthDtos;


namespace Applicant.API.Application.Services.Interfaces
{
    public interface IAccessCodeService
    {
        Task<AuthResult> RegisterUserAsync(AuthRegisterDto authRegisterDto, CancellationToken cancellationToken = default);
        Task<AuthResult> LoginUserAsync(AuthLoginDto authLoginDto, CancellationToken cancellationToken = default);
        Task<AuthResult> RefreshTokenAsync(AuthTokenRequestDto tokenRequest, CancellationToken cancellationToken = default);
        Task AccessCodeAsync(AuthRegisterDto authRegisterDto, CancellationToken cancellationToken = default);

    }

}
