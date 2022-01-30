using System;
using Report.API.Application.Models;
using System.Threading.Tasks;

namespace Report.API.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}