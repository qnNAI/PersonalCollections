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
        Task<IdentityResult> SignUpAsync(SignUpRequest request);

        Task DeleteMultipleAsync(string[] users, CancellationToken cancellationToken = default);
        Task LockMultipleAsync(string[] users, CancellationToken cancellationToken = default);
        Task UnlockMultipleAsync(string[] users, CancellationToken cancellationToken = default);
        Task AddAdminMultipleAsync(string[] users, CancellationToken cancellationToken = default);
        Task RemoveAdminMultipleAsync(string[] users, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserDto>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<int> CountUsersAsync(CancellationToken cancellationToken = default);
    }
}
