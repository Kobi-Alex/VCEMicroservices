using System;
using System.Threading.Tasks;
using Report.API.Application.Models;


namespace Report.API.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}