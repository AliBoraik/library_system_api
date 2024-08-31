using Library.Domain;
using Microsoft.AspNetCore.Http;

namespace Library.Interfaces.Services;

public interface IUploadsService
{
    Task<Result<Ok, Error>> AddFile(string directoryPath, string filePath, IFormFile file);
    Result<Ok, Error> DeleteFile(string filePath);
}