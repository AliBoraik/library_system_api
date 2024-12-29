using Library.Domain;
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
            var fullFilePath = Path.Combine(fullDirectoryPath, fileId) +
                               Path.GetExtension(file.FileName);
            if (!Directory.Exists(fullDirectoryPath)) Directory.CreateDirectory(fullDirectoryPath);
            await using var stream = new FileStream(fullFilePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return fullFilePath;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Error(StatusCodes.Status500InternalServerError, "Can't save file");
        }
    }

    public Result<Ok, Error> DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            return new Ok();
        }
        catch (Exception e)
        {
            return new Error(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}