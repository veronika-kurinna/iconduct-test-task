using iConductTestTask.Server.Models;
using iConductTestTask.Server.Services;
using Npgsql;

namespace iConductTestTask.Server.Data.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;
    private readonly IEmployeeMapper _mapper;

    public EmployeeRepository(IConfiguration config, IEmployeeMapper mapper)
    {
        _connectionString = config.GetConnectionString("iConductTestTaskDb")
            ?? throw new ArgumentNullException("Connection string 'iConductTestTaskDb' not found.");
        _mapper = mapper;
    }

    public async Task<List<Employee>> GetEmployeeByIdWithSubordinates(int employeeId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            WITH RECURSIVE Employees AS (
                SELECT Id, Name, ManagerId
                FROM Employee
                WHERE ID = @employeeId

                UNION ALL

                SELECT e.Id, e.Name, e.ManagerId
                FROM Employee e
                INNER JOIN Employees es ON e.ManagerId = es.Id
            )
            SELECT * FROM Employees;
        ";

        cmd.Parameters.AddWithValue("employeeId", employeeId);

        var result = new List<Employee>();

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(_mapper.MapEmployee(reader));
        }

        return result;
    }

    public async Task UpdateEmployeeEnable(int employeeId, bool enable)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            UPDATE Employee
            SET Enable = @Enable
            WHERE Id = @Id
        ";

        cmd.Parameters.AddWithValue("Enable", enable);
        cmd.Parameters.AddWithValue("Id", employeeId);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();

        if (rowsAffected == 0)
        {
            throw new Exception($"No rows were affected for Employee Id {employeeId}.");
        }
    }
}
