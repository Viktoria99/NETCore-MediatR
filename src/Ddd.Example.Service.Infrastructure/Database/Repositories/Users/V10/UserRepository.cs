using Ddd.Example.Service.Domain.Users.V10;
using Ddd.Example.Service.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Ddd.Example.Service.Infrastructure.Database.Repositories.Users.V10
{

    public class UserRepository : IUserRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly DataContext _context;
        private readonly double _expirationInHours;


        public UserRepository(IMemoryCache memoryCache, DataContext context, IOptionsSnapshot<CacheOptions> cacheOptions)
        {
            _memoryCache = memoryCache;
            _context = context;
            _expirationInHours = cacheOptions?.Value?.ExpirationInHours ?? throw new ArgumentNullException(nameof(cacheOptions.Value.ExpirationInHours));
        }

        public async Task<User> GetUserAsync(int userId)
        {
            if (!_memoryCache.TryGetValue(userId, out User user))
            {
                user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (user != null)
                {
                    _memoryCache.Set(
                        user.Id,
                        user,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(_expirationInHours)));
                }
            }

            return user;
        }

        public async Task<User> FindUserAsync(string name)
        {
            await Task.FromException(new Exception($"Error: {name}."));
            return null;
        }
    }
}
