using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Department;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Ok = Library.Domain.Ok;

namespace Library.Application;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;

    public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    } public async Task<Result<IEnumerable<DepartmentDto>, Error>> GetAllDepartmentsAsync()
    {
        var subjects = await _departmentRepository.GetAllDepartmentsInfoAsync();
        if(!subjects.Any()) {
            return  new Error(StatusCodes.Status404NotFound,"Not found any departments");
        }
        var dto = _mapper.Map<IEnumerable<DepartmentDto>>(subjects);
        return Result<IEnumerable<DepartmentDto>, Error>.Ok(dto);
    }

    public async Task<Result<DepartmentDetailsDto, Error>> GetDepartmentByIdAsync(Guid id)
    {
        var department = await _departmentRepository.GetDepartmentByIdAsync(id);
        if (department == null)
            return new Error(StatusCodes.Status404NotFound ,$"Not found department with id = {id}");
        var departmentDetailsDto = _mapper.Map<DepartmentDetailsDto>(department);
        return departmentDetailsDto;
    }

    public async Task<Result<Guid, Error>> AddDepartmentAsync(CreateDepartmentDto createDepartmentDto)
    {
        var department = _mapper.Map<Department>(createDepartmentDto);
        await _departmentRepository.AddDepartmentAsync(department);
        return department.DepartmentId;
    }

    public async Task<Result<Ok, Error>> UpdateDepartmentAsync(DepartmentDto departmentDto)
    {
        var departmentExists = await _departmentRepository.GetDepartmentByIdAsync((Guid)departmentDto.DepartmentId!);
        if (departmentExists == null)
            return new Error(StatusCodes.Status404NotFound, $"Can't found Department with ID = {departmentDto.DepartmentId}");
        var department = _mapper.Map<Department>(departmentDto);
        await _departmentRepository.UpdateDepartmentAsync(department);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> DeleteDepartmentAsync(Guid id)
    {
        var departmentExists = await _departmentRepository.GetDepartmentByIdAsync(id);
        if (departmentExists == null)
            return new Error( StatusCodes.Status404NotFound, $"Can't found Department with ID = {id}");
        await _departmentRepository.DeleteDepartmentAsync(departmentExists);
        return new Ok();
    }
}