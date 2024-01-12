using Ddd.Example.Service.Domain.Users.V10;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ddd.Example.Service.Infrastructure.Database.Configuration
{

    public class UserConfig : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasColumnName("idUser");

            builder
                .Property(m => m.Login)
                .HasColumnName("sLogin");

            builder
                .Property(m => m.ProfileEQ)
                .HasColumnName("sProfile");

            builder.ToTable("User", "dbo");
        }
    }
}
