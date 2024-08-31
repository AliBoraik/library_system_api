using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Library.Domain.Attributes;

public class MaxFileSizeAttribute(int maxFileSize) : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file) return ValidationResult.Success;
        return file.Length > maxFileSize ? new ValidationResult(GetErrorMessage()) : ValidationResult.Success;
    }

    private string GetErrorMessage()
    {
        return $"Maximum allowed file size is {maxFileSize} bytes.";
    }
}