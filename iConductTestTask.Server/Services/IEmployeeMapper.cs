using iConductTestTask.Server.Dtos;
using iConductTestTask.Server.Models;
using Npgsql;

namespace iConductTestTask.Server.Services;

public interface IEmployeeMapper
{
    public Employee MapEmployee(NpgsqlDataReader reader);
    public EmployeeDto MapEmployeeDto(List<Employee> employees, int employeeId);
}
