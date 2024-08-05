using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {                                           
                                                /* The generic type DbContextOptions<> is added to prevent the exception
                                                 * (since we have more than one DbContext class which have DbcontextOptions
                                                 * passed as paramerters in their constructor which will result in 
                                                 * exception if it is of non generic type)*/
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        //seeding data into the database for roles 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "ae46930c-eeed-4603-9153-d18dae47def7";
            var writerRoleId = "1dc38c61-1a74-48c8-bdca-e10d18a2cdda";
            
            /*ASP.NET Core Identity provides a default implementation of roles through the IdentityRole class. This class is part of the Microsoft.AspNetCore.Identity namespace.
             
              IdentityRole includes properties like Id (role identifier), Name (role name), NormalizedName (normalized role name for efficient lookups), and ConcurrencyStamp (used for concurrency checks).*/

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
