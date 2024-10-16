using BoltCargo.WebUI.Dtos;

namespace BoltCargo.WebUI.Services.Abstracts
{
    public interface IPhotoService
    {
        Task<string> UploadImageAsync(PhotoCreationDto dto);
    }
}
