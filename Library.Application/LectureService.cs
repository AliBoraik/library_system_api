using AutoMapper;
using Library.Domain;
using Library.Domain.Constants;
using Library.Domain.DTOs.Lecture;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Library.Application;

public class LectureService(
    ILectureRepository lectureRepository,
    ISubjectRepository subjectRepository,
    UserManager<User> userManager,
    IMapper mapper,
    IUploadsService uploadsService)
    : ILectureService
{
    public async Task<IEnumerable<LectureResponseDto>> GetAllLecturesAsync()
    {
        var lectures = await lectureRepository.FindAllLecturesAsync();
        return mapper.Map<IEnumerable<LectureResponseDto>>(lectures);
    }

    public async Task<Result<LectureResponseDto, Error>> GetLectureByIdAsync(Guid id)
    {
        var lecture = await lectureRepository.FindLectureWithSubjectByIdAsync(id);
        if (lecture == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found lecture with id = {id}");
        return mapper.Map<LectureResponseDto>(lecture);
    }

    public async Task<Result<Guid, Error>> AddLectureAsync(CreateLectureDto lectureDto, Guid userId)
    {
        var lectureExists = await lectureRepository.FindLectureByNameAsync(lectureDto.Title, lectureDto.SubjectId);
        if (lectureExists != null)
            return new Error(StatusCodes.Status409Conflict, $"Lecture with title = {lectureDto.Title} already exists");
        // check Id 
        var subject = await subjectRepository.FindSubjectByIdAsync(lectureDto.SubjectId);
        if (subject == null)
            return new Error(StatusCodes.Status404NotFound, $"Subject with Id = {lectureDto.SubjectId} not found");
        if (subject.TeacherId != userId)
        {
            // Check if userId is in the Admin role
            if (!await userManager.IsInRoleAsync(new User { Id = userId }, AppRoles.Admin))
            {
                return new Error(StatusCodes.Status403Forbidden, "You don't have access");
            }
        }
        // file info
        var lectureId = Guid.NewGuid();
        // save in disk
        var uploadResult = await uploadsService.AddFile(lectureDto.SubjectId.ToString(), lectureId.ToString(), lectureDto.File);
        if (!uploadResult.IsOk)
            return uploadResult.Error;
        // save in database
        var lecture = mapper.Map<Lecture>(lectureDto);
        lecture.FilePath = uploadResult.Value;
        lecture.Id = lectureId;
        lecture.UploadedBy = userId;
        await lectureRepository.AddLectureAsync(lecture);
        
        return lecture.Id;
    }

    public async Task<Result<Ok, Error>> DeleteLectureAsync(Guid id, Guid teacherId)
    {
        var lecture = await lectureRepository.FindLectureWithSubjectByIdAsync(id);
        if (lecture == null) return new Error(StatusCodes.Status404NotFound, $"Can't found Lecture with ID = {id}");
        if (lecture.Subject.TeacherId != teacherId)
            return new Error(StatusCodes.Status403Forbidden, "Unauthorized to delete");
        await lectureRepository.DeleteLectureAsync(lecture);
        var uploadResult = uploadsService.DeleteFile(lecture.FilePath);
        if (!uploadResult.IsOk)
        {
            // TODO check if Can't delete file from disk 
        }

        return new Ok();
    }

    public async Task<Result<Lecture, Error>> GetLectureFilePathByIdAsync(Guid userId, Guid lectureId)
    {
        var accessToLecture = await HasAccessToLecture(userId, lectureId);
        if (!accessToLecture.IsOk)
            return accessToLecture.Error;
        var lecture = accessToLecture.Value; 
        return lecture;
    }
    
    public async Task<Result<Lecture, Error>> HasAccessToLecture(Guid userId, Guid lectureId)
    {
        var lecture = await lectureRepository.FindLectureWithSubjectByIdAsync(lectureId);
        if (lecture == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found lecture with id = {lectureId}");
        // teacher can access books 
        if (lecture.Subject.TeacherId == userId)
            return lecture;
        // Get the current user
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return new  Error(StatusCodes.Status401Unauthorized,"User not found.");
        // Admins can download any book
        if (await userManager.IsInRoleAsync(user, AppRoles.Admin))
            return lecture;
        // Students can access books linked to their department
        if (user.DepartmentId != lecture.Subject.DepartmentId)
            return new Error(StatusCodes.Status403Forbidden, $"You do not have permission to download this file.");
        return lecture;
    }
}