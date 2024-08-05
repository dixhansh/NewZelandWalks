using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
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
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext nZWalksDbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.nZWalksDbContext = nZWalksDbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        //GET All regions 
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            //Getting data from database - DomainModel
            var regionsDomain = await regionRepository.GetAllAsync();

            //Map DomainModel to DTO
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            return Ok(regionsDto);
        }


        //GET Single Region by ID
        [HttpGet]
        [Route("{id:Guid}")] // :Guid is setting the type of id so that only guid type can be passed
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) //id parameter name should be same as Route's {id}
        {
         
            var regionsDomain = await regionRepository.GetByIdAsync(id);

            if(regionsDomain == null)
            {
                return NotFound();
            }

            //Mapping RegionDomain Model to RegionDto and returning the response
            return Ok(mapper.Map<RegionDto>(regionsDomain));
        }


        //POST to create a new region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {

                //Mapping Dto to Region 
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                //Map just added model to dto to send it back to client
                var RegionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { Id = RegionDto.Id }, RegionDto);

        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //Mapping the updated regionDomainModel to a dto to send it back as response
                return Ok(mapper.Map<RegionDto>(regionDomainModel));
           
        }


        //Delete region 
        //DELETE: https://localhost:portno/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //Mapping deleted RegionDomainModel to dto to send it back as response
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
