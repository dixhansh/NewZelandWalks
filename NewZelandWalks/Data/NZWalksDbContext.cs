using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
                   
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*The base.OnModelCreating(modelBuilder) line is used to call the base class's implementation of OnModelCreating.
             * 
              In Entity Framework Core, the base implementation of OnModelCreating in the DbContext class is typically empty and doesn't perform any actions. However, calling base.OnModelCreating ensures that any base class implementations (if they exist) are executed.*/
            base.OnModelCreating(modelBuilder);

            //creating a list of Diffuculties
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("26cbb49b-ec90-4c2b-8bf6-e1dafa05120d"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id =  Guid.Parse("27468d81-cca2-4b61-9a1c-d9a56f118b86"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("cecc00d7-0961-47f3-9b46-0623dffc2e7c"),
                    Name = "Hard"
                }
            };

            //seed data(difficulties) into the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);


            //seeding region data in region table
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                },
            };

            modelBuilder.Entity<Region>().HasData(regions);

        }
    }

   
}
