using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map addWalkRequestDto to Domain Model i.e Walk
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            //Map DomainModel 
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = walkDto.Id }, walkDto);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomainModelList = await walkRepository.GetAllAsync();

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

            //Map DomainModel to Dto 
            return Ok(mapper.Map<WalkDto>(WalkDomainModel));
        }

        //update walk by id
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map Dto to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            var updatedWalkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if(updatedWalkDomainModel == null)
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
