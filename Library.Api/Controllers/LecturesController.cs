using System.ComponentModel.DataAnnotations;
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
        var result = await _lectureService.GetLectureByIdAsync(id);
        return result.Match<ActionResult<LectureDto>>(
            dto => Ok(dto),
            error => StatusCode(error.Code, error));
    }

    // POST: api/Lectures
    [HttpPost]
    public async Task<ActionResult<LectureDto>> PostLecture([FromForm] CreateLectureDto createLectureDto,
        [Required] IFormFile file)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _lectureService.AddLectureAsync(createLectureDto, file);
        return result.Match<ActionResult>(
            id => CreatedAtAction("GetLecture", new { id }, new { id }),
            error => StatusCode(error.Code, error));
    }

    // PUT: api/Lectures
    [HttpPut]
    public async Task<IActionResult> PutLecture([FromForm] LectureDto lectureDto, IFormFile? file)
    {
        var result = await _lectureService.UpdateLectureAsync(lectureDto, file);
        return result.Match<IActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }

    // DELETE: api/Lectures/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLecture(Guid id)
    {
        var result = await _lectureService.DeleteLectureAsync(id);
        return result.Match<IActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }

    // GET: api/Lectures/download/5
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadLecture(Guid id)
    {
        var result = await _lectureService.GetLectureFilePathByIdAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        var path = result.Value;
        var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(fileBytes, "application/octet-stream", Path.GetFileName(path));
    }
}