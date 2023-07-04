using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Net.Mime;


namespace HomeDatabase.Controllers
{
    public class FileController : Controller
    {

        private readonly IWebHostEnvironment _env;

        public FileController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Download(string fileName)
        {
            
            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Path.Combine(_env.WebRootPath, "Files", fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var fileContent = System.IO.File.ReadAllBytes(filePath);
                var contentType = "application/octet-stream";
                var contentDisposition = new ContentDisposition
                {
                    FileName = fileName,
                    Inline = false
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                return File(fileContent, contentType);
            }
            else
            {
                return NotFound();
            }
                
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile fileInput)
        {
            if (fileInput == null || fileInput.Length == 0)
            {
                return BadRequest("Please select a file to upload.");
            }

            var fileName = Path.GetFileName(fileInput.FileName);
            var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileInput.CopyToAsync(stream);
            }

            return RedirectToAction("Index");
        }

    }
}
