using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto requestDto)
        {
            ValidateFileUpload(requestDto);

            if (ModelState.IsValid) //i.e if the ModelState is free from both the ModelState.AddModelError() 
            {
                //convert DTO to DomainModel
                var imageDomainModel = new Image
                {
                    File = requestDto.File,
                    FileExtension = Path.GetExtension(requestDto.File.FileName),
                    FileSizeInBytes = requestDto.File.Length,
                    FileName = requestDto.FileName,
                    FileDescription = requestDto.FileDescription
                };

                //use repository to upload the image
                await imageRepository.UploadAsync(imageDomainModel);
                return Ok(imageDomainModel);

            }

            return BadRequest(ModelState);

        }

        //Here we will be validating the extension and the size of the file
        private void ValidateFileUpload(ImageUploadRequestDto requestDto)
        {
            var allowedExtensions = new String[] {".jpg",".jpeg",".png" };

            if(!allowedExtensions.Contains(Path.GetExtension(requestDto.File.FileName)))
            {
                ModelState.AddModelError("file", "unsupported file extension");
            }

            if(requestDto.File.Length > 15728640)
            {
                ModelState.AddModelError("file", "File size is more than 10MB");
            }

        }

    }
}
