using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Services
{
    public interface IUserService
    {
        Task<int> CheckOrCreateAsync(string phone);
    }
}
