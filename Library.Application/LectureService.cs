using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Lecture;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class LectureService : ILectureService
{
    private readonly ILectureRepository _lectureRepository;
    private readonly IMapper _mapper;

    public LectureService(ILectureRepository lectureRepository, IMapper mapper)
    {
        _lectureRepository = lectureRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LectureDto>> GetAllLecturesAsync()
    {
        var lectures = await _lectureRepository.FindAllLecturesAsync();
        return _mapper.Map<IEnumerable<LectureDto>>(lectures);
    }

    public async Task<Result<LectureDto, Error>> GetLectureByIdAsync(Guid id)
    {
        var lecture = await _lectureRepository.FindLectureByIdAsync(id);
        if (lecture == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found lecture with id = {id}");
        return _mapper.Map<LectureDto>(lecture);
    }

    public async Task<Result<Guid, Error>> AddLectureAsync(CreateLectureDto lectureDto, IFormFile file)
    {
        var lectureExists = await _lectureRepository.FindLectureByNameAsync(lectureDto.Title);
        if (lectureExists != null)
            return new Error(StatusCodes.Status409Conflict, $"Lecture with title = {lectureDto.Title} already exists");

        var directoryPath = Path.Combine("Uploads", lectureDto.SubjectId.ToString());
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        var filePath = Path.Combine(directoryPath, file.FileName);
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var lecture = _mapper.Map<Lecture>(lectureDto);
        lecture.FilePath = filePath;
        await _lectureRepository.AddLectureAsync(lecture);
        return lecture.LectureId;
    }

    public async Task<Result<string, Error>> GetLectureFilePathByIdAsync(Guid id)
    {
        var lectureFilePath = await _lectureRepository.FindLectureFilePathByIdAsync(id);
        if (lectureFilePath == null)
            return new Error(StatusCodes.Status404NotFound, $"Can't found Lecture with ID = {id}");
        return lectureFilePath;
    }

    public async Task<Result<Ok, Error>> UpdateLectureAsync(LectureDto lectureDto, IFormFile? file)
    {
        var lectureExists = await _lectureRepository.FindLectureByIdAsync((Guid)lectureDto.LectureId!);
        if (lectureExists == null)
            return new Error(StatusCodes.Status404NotFound, $"Can't found Lecture with ID = {lectureDto.LectureId}");
        //TODO test this 
        if (file != null)
        {
            var filePath = Path.Combine("Uploads", file.FileName);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (File.Exists(lectureExists.FilePath)) File.Delete(lectureExists.FilePath);
            lectureExists.FilePath = filePath;
        }

        await _lectureRepository.UpdateLectureAsync(lectureExists);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> DeleteLectureAsync(Guid id)
    {
        var lecture = await _lectureRepository.FindLectureByIdAsync(id);
        if (lecture == null) return new Error(StatusCodes.Status404NotFound, $"Can't found Lecture with ID = {id}");
        if (File.Exists(lecture.FilePath)) File.Delete(lecture.FilePath);
        await _lectureRepository.DeleteLectureAsync(lecture);
        return new Ok();
    }
}