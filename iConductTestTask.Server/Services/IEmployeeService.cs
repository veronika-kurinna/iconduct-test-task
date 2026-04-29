using iConductTestTask.Server.Dtos;

namespace iConductTestTask.Server.Services;

public interface IEmployeeService
{
    Task<EmployeeDto> GetEmployeeWithSubordinates(int employeeId);
    Task UpdateEmployeeEnable(int employeeId, bool enable);
}
