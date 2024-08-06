using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext nZWalksDbContext;

        /*This class provides the information about the web hosting environment
* and the application that is running in it*/
        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NZWalksDbContext nZWalksDbContext )
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Image> UploadAsync(Image image)
        {
            //finding the path to the Image folder and saving it in a variable
            //in order to get the url to the Image folder we will use IWebHostEnvironment
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            //uploading image to the localFilePath
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            //saving uploaded file information into the database
            //https://localhost:1234/Images/new-upload-image.jpg type of url will be stored in the database which points to the saved image in the server.
            //Note that https://localhost:1234/ can be replaced with a web-service url which we want to use to save the images remotely
            //Now in oreder to get https://localhost:1234/ we will need to inject a dependency called HttpContextAccessor.(Add this is program.cs for DI)


            //creating this url--> https://localhost:1234/Images/new-upload-image.jpg
                                    
                                //this will find the type of protocol(https)        //localhost                                                                           //portno
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            //Add the image to the Image table
            await nZWalksDbContext.Images.AddAsync(image);
            await nZWalksDbContext.SaveChangesAsync();
            return image;
        }
    }
}
