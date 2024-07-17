using AutoMapper;
using Library.Domain.DTOs;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;

namespace Library.Application;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;

    public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentInfoDto>> GetAllDepartmentsAsync()
    {
        var subjects = await _departmentRepository.GetAllDepartmentsAsync();
        return _mapper.Map<IEnumerable<DepartmentInfoDto>>(subjects);
    }

    public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
    {
        var department = await _departmentRepository.GetDepartmentByIdAsync(id);
        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task AddDepartmentAsync(DepartmentDto departmentDto)
    {
        var department = _mapper.Map<Department>(departmentDto);
        await _departmentRepository.AddDepartmentAsync(department);
    }

    public async Task UpdateDepartmentAsync(DepartmentDto departmentDto)
    {
        var department = _mapper.Map<Department>(departmentDto);
        await _departmentRepository.UpdateDepartmentAsync(department);
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        await _departmentRepository.DeleteDepartmentAsync(id);
    }
}