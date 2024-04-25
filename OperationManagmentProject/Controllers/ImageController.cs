namespace OperationManagmentProject.Controllers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.IO;

    public class ImageController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public ImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("upload/{userId}")]
        public IActionResult Upload(string userId, IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var relativePath = $"/uploads/{userId}";
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName.Replace(" ", "-");
                    var folderPath = Path.Combine(_environment.WebRootPath, relativePath);
                    var filePath = Path.Combine(folderPath, uniqueFileName);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Get the base URL
                    var externalServerIp = "213.6.49.150:9998";
                    var baseUrl = $"{HttpContext.Request.Scheme}://{externalServerIp}";

                    // Return the full URL of the uploaded image
                    var fullUrl = $"{baseUrl}/uploads/{userId}/{uniqueFileName}";

                    return Ok(new { filePath = fullUrl });
                }

                return BadRequest("Invalid file");
            }
            catch (Exception ex)
            {
                return BadRequest($"Unexpected error while saving the image. error message:{ex.Message}, \n with stackTrace: {ex.StackTrace}");
            }

        }

        [HttpPost("uploadPlan")]
        public IActionResult UploadPlan(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var relativePath = $"/uploads/Plans";
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName.Replace(" ", "-");
                    var folderPath = Path.Combine(_environment.WebRootPath, relativePath);
                    var filePath = Path.Combine(folderPath, uniqueFileName);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Get the base URL
                    var externalServerIp = "213.6.49.150:9998";
                    var baseUrl = $"{HttpContext.Request.Scheme}://{externalServerIp}";

                    // Return the full URL of the uploaded image
                    var fullUrl = $"{baseUrl}/uploads/Plans/{uniqueFileName}";

                    return Ok(new { filePath = fullUrl });
                }

                return BadRequest("Invalid file");
            }
            catch (Exception ex)
            {
                return BadRequest($"Unexpected error while saving the image. error message:{ex.Message}, \n with stackTrace: {ex.StackTrace}");
            }

        }

    }
}
