using System.ComponentModel.DataAnnotations;
using Library.Domain.DTOs;
using Library.Domain.DTOs.Lecture;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LecturesController : ControllerBase
{
    private readonly ILectureService _lectureService;

    public LecturesController(ILectureService lectureService)
    {
        _lectureService = lectureService;
    }

    // GET: api/Lectures
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LectureDto>>> GetLectures()
    {
        var lectures = await _lectureService.GetAllLecturesAsync();
        return Ok(lectures);
    }

    // GET: api/Lectures/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LectureDto>> GetLecture(Guid id)
    {
        var lecture = await _lectureService.GetLectureByIdAsync(id);

        if (lecture == null) return NotFound();

        return Ok(lecture);
    }

    // POST: api/Lectures
    [HttpPost]
    public async Task<ActionResult<LectureDto>> PostLecture([FromForm] CreateLectureDto createLectureDto, [Required] IFormFile file)
    {
        await _lectureService.AddLectureAsync(createLectureDto, file);
        return Ok();
    }

    // PUT: api/Lectures/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLecture(Guid id, [FromForm] LectureDto lectureDto, IFormFile? file)
    {
        if (id != lectureDto.LectureId) return BadRequest();

        await _lectureService.UpdateLectureAsync(lectureDto, file);

        return NoContent();
    }

    // DELETE: api/Lectures/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLecture(Guid id)
    {
        await _lectureService.DeleteLectureAsync(id);
        return NoContent();
    }

    // GET: api/Lectures/download/5
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadLecture(Guid id)
    {
        var path = await _lectureService.GetLectureFilePathByIdAsync(id);
        var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(fileBytes, "application/octet-stream", Path.GetFileName(path));
    }
}