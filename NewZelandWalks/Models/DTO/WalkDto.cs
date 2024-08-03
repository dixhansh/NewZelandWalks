namespace NZWalks.API.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        /* public Guid DifficultyId { get; set; }
         public Guid RegionId { get; set; }*/
        //since we want to show the difficuly and the region of the walk using navigational properties


        public DifficultyDto Difficulty { get; set; }
        public RegionDto Region { get; set; }//since we only send DTO to the client and not the actual Domain Model
        

    }
}
