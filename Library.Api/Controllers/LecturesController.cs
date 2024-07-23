using System.ComponentModel.DataAnnotations;
using Library.Domain.DTOs;
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
    public async Task<ActionResult<LectureDto>> GetLecture(int id)
    {
        var lecture = await _lectureService.GetLectureByIdAsync(id);

        if (lecture == null) return NotFound();

        return Ok(lecture);
    }

    // POST: api/Lectures
    [HttpPost]
    public async Task<ActionResult<LectureDto>> PostLecture([FromForm] LectureDto lectureDto, [Required] IFormFile file)
    {
        await _lectureService.AddLectureAsync(lectureDto, file);
        return Ok();
    }

    // PUT: api/Lectures/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLecture(int id, [FromForm] LectureDto lectureDto, IFormFile? file)
    {
        if (id != lectureDto.LectureId) return BadRequest();

        await _lectureService.UpdateLectureAsync(lectureDto, file);

        return NoContent();
    }

    // DELETE: api/Lectures/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLecture(int id)
    {
        await _lectureService.DeleteLectureAsync(id);
        return NoContent();
    }

    // GET: api/Lectures/download/5
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadLecture(int id)
    {
        var lecture = await _lectureService.GetLectureByIdAsync(id);
        if (lecture == null) return NotFound();

        var path = lecture.FilePath;
        var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(fileBytes, "application/octet-stream", Path.GetFileName(path));
    }
}