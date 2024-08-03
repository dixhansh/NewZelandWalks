using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public SQLWalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await nZWalksDbContext.Walks.AddAsync(walk);
            Console.WriteLine(walk);
            await nZWalksDbContext.SaveChangesAsync();

            return walk;

        }

        public async Task<List<Walk>> GetAllAsync()
        {
            //return await nZWalksDbContext.Walks.ToListAsync(); /*This will not return the navigational properties that are defined in the Walk entity*/
                                                
                                                /*can use any both type of formats:
                                                 Include("NAME_of_navigational_property") or
                                                 Include(w => w.Region).The second one is type safe.
                                                Also: The method takes the Name of the navigational 
                                                properties, not the Type*/
            return await nZWalksDbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(w => w.Id == id);

            if(existingWalk == null)
            {
                return null;
            }

            nZWalksDbContext.Walks.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var existingWalk = await nZWalksDbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(w => w.Id == id);

            if(existingWalk == null)
            {
                return null;
            }
            return existingWalk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(w => w.Id == id);

            if(existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;

            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await nZWalksDbContext.SaveChangesAsync();

            /*extraction updated entity again so but this time with Include() method so that the respose contains the navigations properties valuse and not null*/
            var updateWalk = await nZWalksDbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(w => w.Id == id);  

            return existingWalk;
            
        }
    }
}
