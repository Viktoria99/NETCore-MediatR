using Ddd.Example.Service.Domain.Users.V10;
using Ddd.Example.Service.Infrastructure.Database.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Ddd.Example.Service.Infrastructure.Database
{

    public class DataContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        /// <param name="options"><see cref="DbContextOptions"/></param>
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
        }
    }
}
