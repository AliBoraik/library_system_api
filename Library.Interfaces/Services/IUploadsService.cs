using Library.Domain.Results;
using Library.Domain.Results.Common;
using Microsoft.AspNetCore.Http;

namespace Library.Interfaces.Services;

public interface IUploadsService
{
    Task<Result<string, Error>> AddFile(string directoryPath, string filePath, IFormFile file);
    Result<Ok, Error> DeleteFile(string filePath);
}