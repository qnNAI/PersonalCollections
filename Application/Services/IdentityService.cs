using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Contracts.Services;
using Application.Models.Identity;
using Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{

    internal class IdentityService : IIdentityService {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> SignUpExternalAsync(SignUpExternalRequest request) {
            if(request is null) {
                throw new ArgumentNullException(nameof(request));
            }

            var user = request.Adapt<ApplicationUser>();
            user.ThirdPartyAuth = true;
            user.RegistrationDate = DateTime.UtcNow;
            
            var createdResult = await _userManager.CreateAsync(user);
            return createdResult;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpRequest request) {
            if(request is null) {
                throw new ArgumentNullException(nameof(request));
            }

            var user = request.Adapt<ApplicationUser>();
            user.ThirdPartyAuth = false;
            user.RegistrationDate = DateTime.UtcNow;

            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing is not null) {
                return IdentityResult.Failed(
                    new IdentityError {
                        Code = "EmailAlreadyInUse",
                        Description = "Specified email already in use!"
                    }
                );
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            return result;
        }
    }
}
