using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Department;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class DepartmentService(IDepartmentRepository departmentRepository,  IMapper mapper) : IDepartmentService
{
    public async Task<Result<IEnumerable<DepartmentDto>, Error>> GetAllDepartmentsAsync()
    {
        var subjects = await departmentRepository.FindAllDepartmentsInfoAsync();
        if (!subjects.Any()) return new Error(StatusCodes.Status404NotFound, "Not found any departments");
        var dto = mapper.Map<IEnumerable<DepartmentDto>>(subjects);
        return Result<IEnumerable<DepartmentDto>, Error>.Ok(dto);
    }

    public async Task<Result<DepartmentDetailsDto, Error>> GetUserDepartmentAsync(Guid userId)
    {
        var department = await departmentRepository.FindUserDepartmentAsync(userId);
        if (department == null)
            return new Error(StatusCodes.Status404NotFound, "Not found");
        return mapper.Map<DepartmentDetailsDto>(department);

    }

    public async Task<Result<DepartmentDetailsDto, Error>> GetDepartmentByIdAsync(int id)
    {
        var department = await departmentRepository.FindDepartmentByIdAsync(id);
        if (department == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found department with id = {id}");
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