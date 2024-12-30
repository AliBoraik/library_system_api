using Library.Domain.Results;
using Library.Domain.Results.Common;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class UploadsService : IUploadsService
{
    public async Task<Result<string, Error>> AddFile(string subjectId, string fileId, IFormFile file)
    {
        try
        {
            var baseUploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var fullDirectoryPath = Path.Combine(baseUploadsPath, subjectId);
            var fullFilePath = Path.Combine(fullDirectoryPath, fileId) + Path.GetExtension(file.FileName);
            if (!Directory.Exists(fullDirectoryPath)) Directory.CreateDirectory(fullDirectoryPath);
            await using var stream = new FileStream(fullFilePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return fullFilePath;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<string, Error>.Err(Errors.InternalServerError());
        }
    }

    public Result<Ok, Error> DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            return ResultHelper.Ok();
        }
        catch (Exception e)
        {
            return Result<Ok, Error>.Err(Errors.InternalServerError());
        }
    }
}