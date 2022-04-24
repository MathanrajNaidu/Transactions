using Domain;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class FileUploadController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost("FileUpload")]
        public IActionResult Index(FileUploadModel fileUploadModel)
        {
            if (ModelState.IsValid)
            {
                IFormFile? file = fileUploadModel?.UploadFile;
                if (file?.Length > 0)
                {
                    using var reader = new StreamReader(file.OpenReadStream());
                    var errorMessages = FileUploadService.ProcessFile(reader, Path.GetExtension(file.FileName));
                    if (errorMessages.Count > 0)
                    {
                        return BadRequest(errorMessages);
                    }
                    else return Ok();
                }
            }
            return View();
        }
    }
}
