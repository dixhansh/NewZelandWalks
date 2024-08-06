using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }



        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            
                //Map addWalkRequestDto to Domain Model i.e Walk
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);

                //Map DomainModel 
                var walkDto = mapper.Map<WalkDto>(walkDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = walkDto.Id }, walkDto);
        }

        //Applying Filtering+Sorting+Pagination on GetAll()
        //GET: https://localhost:portno/api/walks?filterOn=Name&filterQuery=parks&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] String? filterOn, [FromQuery] String? filterQuery, [FromQuery] String? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber =1, [FromQuery] int pageSize=1000) //setting pageNumber default to 1 and pageSize to 1000 (unless specified)
        {  
                                                                                                         /*since repository method only accepts
                                                                                                         not null bool therefore setting its 
                                                                                                         default to true*/   
            var walksDomainModelList = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            //Map domain model to dto 
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModelList));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deltedWalkDomainModel = await walkRepository.DeleteAsync(id);

            if(deltedWalkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Model to Dto
            return Ok(mapper.Map<WalkDto>(deltedWalkDomainModel));
        }

        //Get single walk by its id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var WalkDomainModel = await walkRepository.GetByIdAsync(id);
            if(WalkDomainModel == null)
            {
                return NotFound();
            }

            //Mimicking an exception occured during the execution of the code
            throw new Exception("A new exception occured in GetById() method");

            //Map DomainModel to Dto 
            return Ok(mapper.Map<WalkDto>(WalkDomainModel));
        }

        //update walk by id
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            
                //Map Dto to Domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                var updatedWalkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

                if (updatedWalkDomainModel == null)
                {
                    return NotFound();
                }

                //Map Updated Domain Model to Dto
                var updatedWalkDto = mapper.Map<WalkDto>(updatedWalkDomainModel);
                /*return CreatedAtAction(nameof(GetById), new { id = updatedWalkDto.Id, updatedWalkDto});*/

                return Ok(updatedWalkDto);
            
        }
    }
}
