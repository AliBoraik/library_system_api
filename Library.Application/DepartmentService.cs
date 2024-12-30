using AutoMapper;
using Library.Domain.DTOs.Department;
using Library.Domain.Models;
using Library.Domain.Results;
using Library.Domain.Results.Common;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper) : IDepartmentService
{
    public async Task<Result<IEnumerable<DepartmentDto>, Error>> GetAllDepartmentsAsync()
    {
        var departments = await departmentRepository.FindAllDepartmentsInfoAsync();
        var dto = mapper.Map<IEnumerable<DepartmentDto>>(departments);
        return Result<IEnumerable<DepartmentDto>, Error>.Ok(dto);
    }

    public async Task<Result<IEnumerable<DepartmentDto>, Error>> GetAllUserDepartmentsAsync(Guid userId)
    {
        var departments = await departmentRepository.FindAllUserDepartmentsAsync(userId);
        var dto = mapper.Map<IEnumerable<DepartmentDto>>(departments);
        return Result<IEnumerable<DepartmentDto>, Error>.Ok(dto);
    }

    public async Task<Result<DepartmentDetailsDto, Error>> GetDepartmentByIdAsync(int departmentId)
    {
        var department = await departmentRepository.FindDepartmentByIdAsync(departmentId);
        if (department == null)
            return Result<DepartmentDetailsDto, Error>.Err(Errors.NotFound("department"));
        var dto = mapper.Map<DepartmentDetailsDto>(department);
        return Result<DepartmentDetailsDto, Error>.Ok(dto);
    }

    public async Task<Result<DepartmentDetailsDto, Error>> GetUserDepartmentByIdAsync(Guid userId, int departmentId)
    {
        var department = await departmentRepository.FindUserDepartmentByIdAsync(userId, departmentId);
        if (department == null)
            return Result<DepartmentDetailsDto, Error>.Err(Errors.NotFound("department"));
        var dto = mapper.Map<DepartmentDetailsDto>(department);
        return Result<DepartmentDetailsDto, Error>.Ok(dto);
    }

    public async Task<Result<int, Error>> AddDepartmentAsync(CreateDepartmentDto createDepartmentDto)
    {
        var department = mapper.Map<Department>(createDepartmentDto);
        var departmentExists = await departmentRepository.FindDepartmentByIdAsync(department.Id);
        if (departmentExists != null)
            return Result<int, Error>.Err(Errors.Conflict("department"));
        await departmentRepository.AddDepartmentAsync(department);
        return Result<int, Error>.Ok(department.Id);
    }

    public async Task<Result<Ok, Error>> UpdateDepartmentAsync(DepartmentDto departmentDto)
    {
        var departmentExists = await departmentRepository.DepartmentExistsAsync(departmentDto.Id);
        if (!departmentExists)
            return Result<Ok, Error>.Err(Errors.NotFound("department"));
        var department = mapper.Map<Department>(departmentDto);
        await departmentRepository.UpdateDepartmentAsync(department);
        return ResultHelper.Ok();
    }

    public async Task<Result<Ok, Error>> DeleteDepartmentAsync(int id)
    {
        var departmentExists = await departmentRepository.FindDepartmentByIdAsync(id);
        if (departmentExists == null)
            return new Error(StatusCodes.Status404NotFound, $"Can't found Department with ID = {id}");
        await departmentRepository.DeleteDepartmentAsync(departmentExists);
        return ResultHelper.Ok();
    }
}