using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Lecture;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class LectureService(ILectureRepository lectureRepository,ISubjectRepository subjectRepository, IMapper mapper, IUploadsService uploadsService)
    : ILectureService
{
    public async Task<IEnumerable<LectureResponseDto>> GetAllLecturesAsync()
    {
        var lectures = await lectureRepository.FindAllLecturesAsync();
        return mapper.Map<IEnumerable<LectureResponseDto>>(lectures);
    }

    public async Task<Result<LectureResponseDto, Error>> GetLectureByIdAsync(Guid id)
    {
        var lecture = await lectureRepository.FindLectureByIdAsync(id);
        if (lecture == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found lecture with id = {id}");
        return mapper.Map<LectureResponseDto>(lecture);
    }

    public async Task<Result<Guid, Error>> AddLectureAsync(CreateLectureDto lectureDto, string userId)
    {
        
        var lectureExists = await lectureRepository.FindLectureByNameAsync(lectureDto.Title , lectureDto.SubjectId);
        if (lectureExists != null)
            return new Error(StatusCodes.Status409Conflict, $"Lecture with title = {lectureDto.Title} already exists");
        // check SubjectId 
        var subject = await subjectRepository.FindSubjectByIdAsync(lectureDto.SubjectId);
        if (subject == null) 
            return new Error(StatusCodes.Status404NotFound, $"Subject with Id = {lectureDto.SubjectId} not found");
        if (subject.TeacherId != userId) 
            return new Error(StatusCodes.Status403Forbidden, $"You don't have access");
        // file info
        var lectureId = Guid.NewGuid();
        var fullDirectoryPath = Path.Combine("Uploads", lectureDto.SubjectId.ToString());
        var fullFilePath = Path.Combine(fullDirectoryPath, lectureId.ToString()) + Path.GetExtension(lectureDto.File.FileName);
        // save in database
        var lecture = mapper.Map<Lecture>(lectureDto);
        lecture.FilePath = fullFilePath;
        lecture.LectureId = lectureId;
        lecture.UploadedBy = userId;
        await lectureRepository.AddLectureAsync(lecture);
        // save in disk
        var uploadResult = await uploadsService.AddFile(fullDirectoryPath, fullFilePath, lectureDto.File);
        if (uploadResult.IsOk) return lecture.LectureId;
        await lectureRepository.DeleteLectureAsync(lecture);
        return uploadResult.Error;
    }
    public async Task<Result<Ok, Error>> DeleteLectureAsync(Guid id , string teacherId)
    {
        var lecture = await lectureRepository.FindLectureByIdAsync(id);
        if (lecture == null) return new Error(StatusCodes.Status404NotFound, $"Can't found Lecture with ID = {id}");
        if (lecture.Subject.TeacherId != teacherId) return new Error(StatusCodes.Status403Forbidden , "Unauthorized to delete");
        await lectureRepository.DeleteLectureAsync(lecture);
        var uploadResult = uploadsService.DeleteFile(lecture.FilePath);
        if (!uploadResult.IsOk)
        {
            // TODO check if Can't delete file from disk 
        }

        return new Ok();
    }

    public async Task<Result<string, Error>> GetLectureFilePathByIdAsync(Guid id)
    {
        var lectureFilePath = await lectureRepository.FindLectureFilePathByIdAsync(id);
        if (lectureFilePath == null)
            return new Error(StatusCodes.Status404NotFound, $"Can't found Lecture with ID = {id}");
        return lectureFilePath;
    }
}