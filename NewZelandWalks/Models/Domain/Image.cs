using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class Image
    {
        public Guid id { get; set; }
        
        [NotMapped] //since we are not going to save the image in the database
        public IFormFile File  { get; set; }//The file that is beign received in the request is represented by IFormFile Type

        public string FileName { get; set; }

        public string? FileDescription { get; set; }

        public string FileExtension { get; set; }

        public long FileSizeInBytes { get; set; }
         
        public string FilePath { get; set; }

    }
}
