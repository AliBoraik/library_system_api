using AutoMapper;
using Library.Application.Exceptions;
using Library.Domain.DTOs.Department;
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

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var subjects = await _departmentRepository.GetAllDepartmentsInfoAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(subjects);
    }

    public async Task<DepartmentDetailsDto> GetDepartmentByIdAsync(Guid id)
    {
        /*var cacheValue = await _redisCacheService.GetCacheValueAsync(id.ToString());
        if (cacheValue is { IsNullOrEmpty: false, HasValue: true })
        {
            var cacheDepartmentDetailsDto = JsonSerializer.Deserialize<DepartmentDetailsDto>(cacheValue!)!;
            return cacheDepartmentDetailsDto;
        }*/

        var department = await _departmentRepository.GetDepartmentByIdAsync(id);
        if (department == null)
            throw new NotFoundException($"Not found department with id = {id}");
        var departmentDetailsDto = _mapper.Map<DepartmentDetailsDto>(department);
       // await _redisCacheService.SetCacheValueAsync(id.ToString(), JsonSerializer.Serialize(departmentDetailsDto));
        return departmentDetailsDto;
    }

    public async Task<Guid> AddDepartmentAsync(CreateDepartmentDto createDepartmentDto)
    {
        var department = _mapper.Map<Department>(createDepartmentDto);
        await _departmentRepository.AddDepartmentAsync(department);
        return department.DepartmentId;
    }

    public async Task UpdateDepartmentAsync(DepartmentDto departmentDto)
    {
        var department = _mapper.Map<Department>(departmentDto);
        await _departmentRepository.UpdateDepartmentAsync(department);
    }

    public async Task DeleteDepartmentAsync(Guid id)
    {
        await _departmentRepository.DeleteDepartmentAsync(id);
    }
}