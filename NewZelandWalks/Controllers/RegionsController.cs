using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private NZWalksDbContext nZWalksDbContext;

        public RegionsController(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        //GET All regions 
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //Getting data from database - DomainModel
            var regionsDomain = nZWalksDbContext.Regions.ToList();

            //Mapping DomainModel to DtoModel
            var regionsDto = new List<RegionDto>();
            foreach(var rd in regionsDomain)
            {
                regionsDto.Add(new RegionDto()  // Using default constructor and property initializers
                {
                    Id = rd.Id,
                    Code = rd.Code,
                    Name = rd.Name,
                    RegionImageUrl = rd.RegionImageUrl
                });
            }

            return Ok(regionsDto);
        }

        //GET Single Region by ID
        [HttpGet]
        [Route("{id:Guid}")] // :Guid is setting the type of id so that only guid type can be passed
        public IActionResult GetById([FromRoute] Guid id) //id parameter name should be same as Route's {id}
        {
            //Getting RegionDomain model from DB
            var regionsDomain = nZWalksDbContext.Regions.Find(id); //Find() mtd only used for primary key

            //var region = nZWalksDbContext.Regions.FirstOrDefault(r => r.Id == id); //this method can be used for other columns also
            
           
            if(regionsDomain == null)
            {
                return NotFound();
            }

            //Mapping RegionDomain Model to RegionDto
            var regionDto = new RegionDto()
            {
                Id = regionsDomain.Id,
                Code = regionsDomain.Code,
                Name = regionsDomain.Name,
                RegionImageUrl = regionsDomain.RegionImageUrl
            };
            
            return Ok(regionDto);
        }

        //POST to create a new region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Mapping Dto to Region 
            var regionDomainModel = new Region()
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            //storing Region in database
            nZWalksDbContext.Regions.Add(regionDomainModel);
            nZWalksDbContext.SaveChanges();

            //Map just added model to dto to send it back to client
            var RegionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { Id = RegionDto.Id }, RegionDto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Checking if id exist in the database
            var regionDomainModel = nZWalksDbContext.Regions.FirstOrDefault(r => r.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //updating regionDomainModel
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            //we dont have to use the nZWalksDbContext.Update() method since regionDomainModel is alredy being tracked by the entitiy framework when we used FirstOrDefault() method to get it.
            nZWalksDbContext.SaveChanges();

            //Mapping the updated regionDomainModel to a dto to send it back as response

            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }

        //Delete region 
        //DELETE: https://localhost:portno/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            //Finding if id exists in the database
            var regionDomainModel = nZWalksDbContext.Regions.FirstOrDefault(r => r.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //deleting from the database
            nZWalksDbContext.Regions.Remove(regionDomainModel);
            nZWalksDbContext.SaveChanges();

            //Mapping deleted RegionDomainModel to dto to send it back as response
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDomainModel);
        }
    }
}
