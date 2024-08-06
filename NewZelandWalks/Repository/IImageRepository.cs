using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public interface IImageRepository
    {
        Task<Image> UploadAsync(Image image);
    }
}
