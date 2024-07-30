using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext nZWalksDbContext, IRegionRepository regionRepository)
        {
            this.nZWalksDbContext = nZWalksDbContext;
            this.regionRepository = regionRepository;
        }


        //GET All regions 
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public async  Task<IActionResult> GetAll()
        {
            //Getting data from database - DomainModel
            var regionsDomain = await regionRepository.GetAllAsync();

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
        public async Task<IActionResult> GetById([FromRoute] Guid id) //id parameter name should be same as Route's {id}
        {
            //Getting RegionDomain model from DB
            var regionsDomain = await regionRepository.GetByIdAsync(id); //Find() mtd only used for primary key

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
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Mapping Dto to Region 
            var regionDomainModel = new Region()
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

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
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = new Region()
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            };

           regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }


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
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //Mapping deleted RegionDomainModel to dto to send it back as response
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
