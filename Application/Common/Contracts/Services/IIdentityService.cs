using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Contracts.Services
{

    public interface IIdentityService
    {
        //Task<IdentityResult> SignUpExternalAsync(SignUpExternalRequest request);
        Task<IdentityResult> SignUpAsync(SignUpRequest request);
    }
}
