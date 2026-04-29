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
}
