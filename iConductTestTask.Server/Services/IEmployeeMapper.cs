using iConductTestTask.Server.Models;
using Npgsql;

namespace iConductTestTask.Server.Services;

public interface IEmployeeMapper
{
    public Employee MapEmployee(NpgsqlDataReader reader);
}
