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

        public async Task<List<Walk>> GetAllAsync(String? filterOn = null, String? filterQuery = null, String? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            //return await nZWalksDbContext.Walks.ToListAsync(); /*This will not return the navigational properties that are defined in the Walk entity*/

            /*can use any both type of formats:
             Include("NAME_of_navigational_property") or
             Include(w => w.Region).The second one is type safe.
            Also: The method takes the Name of the navigational 
            properties, not the Type*/
            // return await nZWalksDbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();





            
            /*Preparing query: Here we are setting the fetching to eager by telling Include() results of Difficulty and Region to the dataset asked by the query 'walks'.
                               Also we are making the walks query as IQueryable so that we can use the LINQ methods with the result */                 
            var walks = nZWalksDbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //here walks is an IQueryable<Walk> that represents a query to retrieve Walk entities from the nZWalksDbContext database context.

            //FILTERING the walks query only if there are valid parameters in the query string
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {                       //we can do this for other columns too
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(w => w.Name.Contains(filterQuery));
                    //walks is still an IQueryable<Walk>, but it may now include additional filtering logic
                }
            }

            //SORTING the walks query only if there are valid parameters in the query string
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
                }
                else if(sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
                }
            }

            //PAGINATION
            var skipResult = (pageNumber - 1) * pageSize;//pagination is based on this formula

            //ToListAsync() will fire the query to get all walks (+ fetch type eager + the filtering criteria) and returning a List<Walks>
            return await walks.Skip(skipResult).Take(pageSize).ToListAsync(); //Skip() will skip the specified no of results and Take() will take specified no of results(which is page size) 
            /*The query is executed against the database.
              The return value is a List<Walk> containing the Walk entities that match the query criteria,
              including the related Difficulty and Region entities. */
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
