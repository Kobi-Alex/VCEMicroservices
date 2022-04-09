using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Applicant.API.Application.Contracts.Dtos.UserDtos;
using System.Linq.Expressions;

namespace Applicant.API.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UserReadDto> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<UserReadDto> CreateAsync(UserCreateDto userCreateDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(string id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default);
        Task UpdateEmailAsync(UserChangeEmailDto userChangeEmailDto, CancellationToken cancellationToken = default);
        Task<bool> AccessCodeAsync(UserEmailDto userEmailDto, CancellationToken cancellationToken = default);
        Task ChangePassword(UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task AddRoleAsync(UserRoleDto userRoleDto, CancellationToken cancellationToken = default);
        Task RemoveRoleAsync(UserRoleDto userRoleDto, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserExamDto>> GetUserExamsAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserExamReadDto>> GetExamUsersAsync(int examId, CancellationToken cancellationToken = default);
        Task AddExamToUserAsync(UserExamDto userExamDto, CancellationToken cancellationToken = default);
        Task RemoveExamFromUser(UserExamDto userExamDto, CancellationToken cancellationToken = default);
    }
}
