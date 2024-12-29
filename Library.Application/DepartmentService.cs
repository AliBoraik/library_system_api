using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Department;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper) : IDepartmentService
{
    public async Task<Result<IEnumerable<DepartmentDto>, Error>> GetAllDepartmentsAsync()
    {
        var departments = await departmentRepository.FindAllDepartmentsInfoAsync();
        if (!departments.Any()) return new Error(StatusCodes.Status404NotFound, "Not found any departments");
        var dto = mapper.Map<IEnumerable<DepartmentDto>>(departments);
        return Result<IEnumerable<DepartmentDto>, Error>.Ok(dto);
    }

    public async Task<Result<IEnumerable<DepartmentDto>, Error>> GetAllUserDepartmentsAsync(Guid userId)
    {
        var departments = await departmentRepository.FindAllUserDepartmentsAsync(userId);
        if (!departments.Any()) return new Error(StatusCodes.Status404NotFound, "Not found any departments");
        var dto = mapper.Map<IEnumerable<DepartmentDto>>(departments);
        return Result<IEnumerable<DepartmentDto>, Error>.Ok(dto);
    }

    public async Task<Result<DepartmentDetailsDto, Error>> GetDepartmentByIdAsync(int departmentId)
    {
        var department = await departmentRepository.FindDepartmentByIdAsync(departmentId);
        if (department == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found department with departmentId = {departmentId}");
        return mapper.Map<DepartmentDetailsDto>(department);
    }

    public async Task<Result<DepartmentDetailsDto, Error>> GetUserDepartmentByIdAsync(Guid userId, int departmentId)
    {
        var department = await departmentRepository.FindUserDepartmentByIdAsync(userId, departmentId);
        if (department == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found department with id = {departmentId}");
        return mapper.Map<DepartmentDetailsDto>(department);

    }

    public async Task<Result<int, Error>> AddDepartmentAsync(CreateDepartmentDto createDepartmentDto)
    {
        var department = mapper.Map<Department>(createDepartmentDto);
        var departmentExists = await departmentRepository.FindDepartmentByIdAsync(department.Id);
        if (departmentExists != null)
            return new Error(StatusCodes.Status409Conflict, "Department already exists");
        await departmentRepository.AddDepartmentAsync(department);
        return department.Id;
    }

    public async Task<Result<Ok, Error>> UpdateDepartmentAsync(DepartmentDto departmentDto)
    {
        var departmentExists = await departmentRepository.DepartmentExistsAsync(departmentDto.Id);
        if (!departmentExists)
            return new Error(StatusCodes.Status404NotFound,
                $"Can't found Department with ID = {departmentDto.Id}");
        var department = mapper.Map<Department>(departmentDto);
        await departmentRepository.UpdateDepartmentAsync(department);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> DeleteDepartmentAsync(int id)
    {
        var departmentExists = await departmentRepository.FindDepartmentByIdAsync(id);
        if (departmentExists == null)
            return new Error(StatusCodes.Status404NotFound, $"Can't found Department with ID = {id}");
        await departmentRepository.DeleteDepartmentAsync(departmentExists);
        return new Ok();
    }
}