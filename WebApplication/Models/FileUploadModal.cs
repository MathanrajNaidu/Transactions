using System.ComponentModel.DataAnnotations;
using WebApplication2.Shared.ValidationAttributes;

namespace WebApplication2.Models

{
    public class FileUploadModel
    {
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(1024 * 1024)]
        [AllowedFileFormat(new string[] { ".csv", ".xml" })]
        public IFormFile? UploadFile { get; set; }
    }
}
