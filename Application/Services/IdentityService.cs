using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Contracts.Contexts;
using Application.Common.Contracts.Services;
using Application.Models.Identity;
using Domain.Entities.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{

    internal class IdentityService : IIdentityService {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;

        public IdentityService(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpRequest request) {
            if(request is null) {
                throw new ArgumentNullException(nameof(request));
            }

            var user = request.Adapt<ApplicationUser>();
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

            if (result.Succeeded) {
                var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
                return addToRoleResult;
            }

            return result;
        }

        public async Task DeleteMultipleAsync(string[] users, CancellationToken cancellationToken = default) {
            this._context.Users.RemoveRange(
                await _GetUsersByIdAsync(users, cancellationToken));

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task LockMultipleAsync(string[] users, CancellationToken cancellationToken = default) {
            var existingUsers = await _GetUsersByIdAsync(users, cancellationToken);
            foreach(var user in existingUsers) {
                user.IsActive = false;
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UnlockMultipleAsync(string[] users, CancellationToken cancellationToken = default) {
            var existingUsers = await _GetUsersByIdAsync(users, cancellationToken);
            foreach(var user in existingUsers) {
                user.IsActive = true;
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAdminMultipleAsync(string[] users, CancellationToken cancellationToken = default) {
            var existingUsers = await _GetUsersFromManagerByIdAsync(users, cancellationToken);
            foreach(var user in existingUsers) {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        public async Task RemoveAdminMultipleAsync(string[] users, CancellationToken cancellationToken = default) {
            var existingUsers = await _GetUsersFromManagerByIdAsync(users, cancellationToken);
            foreach(var user in existingUsers) {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default) {
            var users = await _userManager.Users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            var result = new List<UserDto>();
            foreach (var user in users) {
                var dto = user.Adapt<UserDto>();
                dto.Roles = await _userManager.GetRolesAsync(user);
                result.Add(dto);
            }

            return result;
        }

        public Task<int> CountUsersAsync(CancellationToken cancellationToken = default) {
            return _userManager.Users.CountAsync(cancellationToken);
        }

        private async Task<List<ApplicationUser>> _GetUsersByIdAsync(string[] users, CancellationToken cancellationToken = default) {
            return await _context.Users.Where(x => users.Contains(x.Id)).ToListAsync(cancellationToken);
        }

        private async Task<List<ApplicationUser>> _GetUsersFromManagerByIdAsync(string[] users, CancellationToken cancellationToken = default) {
            return await _userManager.Users.Where(x => users.Contains(x.Id)).ToListAsync(cancellationToken);
        }
    }
}
