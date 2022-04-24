using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Shared.ValidationAttributes
{
    public class AllowedFileFormatAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedFileFormatAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult($"Unknown format");
                }
            }

            return ValidationResult.Success;
        }
    }
}
