using BoltCargo.WebUI.Dtos;
using BoltCargo.WebUI.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoltCargo.WebUI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        public ImageController(IPhotoService photoService) 
        {
            _photoService = photoService;
        }
         
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var file = Request.Form.Files.GetFile("file");

            if (file != null && file.Length > 0)
            {
                string result = await _photoService.UploadImageAsync(new PhotoCreationDto { File = file });
                return Ok(new { ImageUrl = result }); 
            }
            return BadRequest(new { Message = "Photo Creation Failed!" });
        }
    }
}
