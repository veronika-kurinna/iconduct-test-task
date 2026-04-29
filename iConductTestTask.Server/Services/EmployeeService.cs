using iConductTestTask.Server.Data.Repositories;
using iConductTestTask.Server.Dtos;

namespace iConductTestTask.Server.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeDto> GetEmployeeWithSubordinates(int employeeId)
    {
        var employees = await _repository.GetEmployeeByIdWithSubordinates(employeeId);
        if (employees.Count == 0)
        {
            throw new Exception($"Employee with Id {employeeId} was not found.");
        }

        var dictionary = employees.ToDictionary(
            e => e.Id,
            e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                ManagerId = e.ManagerId,
                Subordinates = new List<EmployeeDto>()
            }
        );

        foreach (var emp in employees)
        {
            if (emp.ManagerId == null)
                continue;

            if (dictionary.TryGetValue(emp.ManagerId.Value, out var manager))
            {
                manager.Subordinates.Add(dictionary[emp.Id]);
            }
        }

        return dictionary[employeeId];
    }

    public async Task UpdateEmployeeEnable(int employeeId, bool enable)
    {
        await _repository.UpdateEmployeeEnable(employeeId, enable);
    }
}
