using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpGet("{imageName}")]
        [Authorize]
        public async Task<IActionResult> GetImage(string imageName)
        {
            var imagePath = Path.Combine("wwwhelpers", "Images", imageName);

            if (!System.IO.File.Exists(imagePath))
                return NotFound();

            try
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                var mimeType = GetMimeType(imagePath);
                return File(memory, mimeType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        private string GetMimeType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }

    }
}
