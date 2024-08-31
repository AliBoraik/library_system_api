using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Lecture;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class LectureService(ILectureRepository lectureRepository, IMapper mapper, IUploadsService uploadsService)
    : ILectureService
{
    public async Task<IEnumerable<LectureDto>> GetAllLecturesAsync()
    {
        var lectures = await lectureRepository.FindAllLecturesAsync();
        return mapper.Map<IEnumerable<LectureDto>>(lectures);
    }

    public async Task<Result<LectureDto, Error>> GetLectureByIdAsync(Guid id)
    {
        var lecture = await lectureRepository.FindLectureByIdAsync(id);
        if (lecture == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found lecture with id = {id}");
        return mapper.Map<LectureDto>(lecture);
    }

    public async Task<Result<Guid, Error>> AddLectureAsync(CreateLectureDto lectureDto, IFormFile file)
    {
        var lectureExists = await lectureRepository.FindLectureByNameAsync(lectureDto.Title , lectureDto.SubjectId);
        if (lectureExists != null)
            return new Error(StatusCodes.Status409Conflict, $"Lecture with title = {lectureDto.Title} already exists");
        // TODO check SubjectId and UploadedBy are exists...
        // file info
        var lectureId = Guid.NewGuid();
        var fullDirectoryPath = Path.Combine("Uploads", lectureDto.SubjectId.ToString());
        var fullFilePath = Path.Combine(fullDirectoryPath, lectureId.ToString()) + Path.GetExtension(file.FileName);
        // save in database
        var lecture = mapper.Map<Lecture>(lectureDto);
        lecture.FilePath = fullFilePath;
        lecture.LectureId = lectureId;
        await lectureRepository.AddLectureAsync(lecture);
        // save in disk
        var uploadResult = await uploadsService.AddFile(fullDirectoryPath, fullFilePath, file);
        if (uploadResult.IsOk) return lecture.LectureId;
        await lectureRepository.DeleteLectureAsync(lecture);
        return uploadResult.Error;
    }

    public async Task<Result<string, Error>> GetLectureFilePathByIdAsync(Guid id)
    {
        var lectureFilePath = await lectureRepository.FindLectureFilePathByIdAsync(id);
        if (lectureFilePath == null)
            return new Error(StatusCodes.Status404NotFound, $"Can't found Lecture with ID = {id}");
        return lectureFilePath;
    }

    public async Task<Result<Ok, Error>> DeleteLectureAsync(Guid id)
    {
        var lecture = await lectureRepository.FindLectureByIdAsync(id);
        if (lecture == null) return new Error(StatusCodes.Status404NotFound, $"Can't found Lecture with ID = {id}");
        await lectureRepository.DeleteLectureAsync(lecture);
        var uploadResult = uploadsService.DeleteFile(lecture.FilePath);
        if (!uploadResult.IsOk)
        {
            // TODO check if Can't delete file from disk 
        }

        return new Ok();
    }
}