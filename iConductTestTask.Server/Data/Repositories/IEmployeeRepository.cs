using iConductTestTask.Server.Models;

namespace iConductTestTask.Server.Data.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetEmployeeByIdWithSubordinates(int employeeId);
    Task UpdateEmployeeEnable(int employeeId, bool enable);
}
