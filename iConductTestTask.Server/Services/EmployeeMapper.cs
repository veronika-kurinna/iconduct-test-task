using iConductTestTask.Server.Dtos;
using iConductTestTask.Server.Models;
using Npgsql;

namespace iConductTestTask.Server.Services;

public class EmployeeMapper : IEmployeeMapper
{
    public Employee MapEmployee(NpgsqlDataReader reader)
    {
        return new Employee
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            ManagerId = reader.IsDBNull(2) ? null : reader.GetInt32(2)
        };
    }

    public EmployeeDto MapEmployeeDto(List<Employee> employees, int employeeId)
    {
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
}
