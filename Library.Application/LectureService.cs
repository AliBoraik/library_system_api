using AutoMapper;
using Library.Domain.DTOs;
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
        var lectures = await _lectureRepository.GetAllLecturesAsync();
        return _mapper.Map<IEnumerable<LectureDto>>(lectures);
    }

    public async Task<LectureDto?> GetLectureByIdAsync(int id)
    {
        var lecture = await _lectureRepository.GetLectureByIdAsync(id);
        return _mapper.Map<LectureDto>(lecture);
    }

    public async Task AddLectureAsync(LectureDto lectureDto, IFormFile file)
    {
        var directoryPath = Path.Combine("Uploads", lectureDto.SubjectId);
        var filePath = Path.Combine(directoryPath, file.FileName);

        // Ensure the directory exists
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        lectureDto.FilePath = filePath;

        var lecture = _mapper.Map<Lecture>(lectureDto);
        await _lectureRepository.AddLectureAsync(lecture);
    }

    public async Task UpdateLectureAsync(LectureDto lectureDto, IFormFile? file)
    {
        if (file != null)
        {
            var filePath = Path.Combine("Uploads", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var oldLecture = await _lectureRepository.GetLectureByIdAsync(lectureDto.LectureId);
            if (oldLecture != null && File.Exists(oldLecture.FilePath)) File.Delete(oldLecture.FilePath);

            lectureDto.FilePath = filePath;
        }

        var lecture = _mapper.Map<Lecture>(lectureDto);
        await _lectureRepository.UpdateLectureAsync(lecture);
    }

    public async Task DeleteLectureAsync(int id)
    {
        var lecture = await _lectureRepository.GetLectureByIdAsync(id);
        if (lecture != null && File.Exists(lecture.FilePath)) File.Delete(lecture.FilePath);
        await _lectureRepository.DeleteLectureAsync(id);
    }
}