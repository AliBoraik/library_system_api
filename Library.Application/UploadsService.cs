using Library.Domain;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class UploadsService : IUploadsService
{
    public async Task<Result<Ok, Error>> AddFile(string fullDirectoryPath, string fullFilePath, IFormFile file)
    {
        try
        {
            if (!Directory.Exists(fullDirectoryPath)) Directory.CreateDirectory(fullDirectoryPath);
            await using var stream = new FileStream(fullFilePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return new Ok();
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