using iConductTestTask.Server.Data.Repositories;
using iConductTestTask.Server.Dtos;

namespace iConductTestTask.Server.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IEmployeeMapper _mapper;

    public EmployeeService(IEmployeeRepository repository, IEmployeeMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> GetEmployeeWithSubordinates(int employeeId)
    {
        var employees = await _repository.GetEmployeeByIdWithSubordinates(employeeId);
        if (employees.Count == 0)
        {
            throw new Exception($"Employee with Id {employeeId} was not found.");
        }

        return _mapper.MapEmployeeDto(employees, employeeId);
    }

    public async Task UpdateEmployeeEnable(int employeeId, bool enable)
    {
        await _repository.UpdateEmployeeEnable(employeeId, enable);
    }
}
