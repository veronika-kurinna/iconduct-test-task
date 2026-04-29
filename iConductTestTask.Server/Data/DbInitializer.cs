using iConductTestTask.Server.Data.Entities;
using Npgsql;

namespace iConductTestTask.Server.Data;

public class DbInitializer
{
    private readonly string _connectionString;

    public DbInitializer(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("iConductTestTaskDb")
                ?? throw new ArgumentNullException("Connection string 'iConductTestTaskDb' not found.");
    }

    public async Task InitializeAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await CreateTable(connection);
        await SeedData(connection);
    }

    private async Task CreateTable(NpgsqlConnection connection)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Employee (
                Id INT PRIMARY KEY,
                Name VARCHAR(255) NOT NULL,
                ManagerId INT NULL REFERENCES Employee(Id),
                Enable BOOLEAN NOT NULL DEFAULT TRUE
            );
        ";

        await cmd.ExecuteNonQueryAsync();
    }

    private async Task SeedData(NpgsqlConnection connection)
    {
        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = "SELECT 1 FROM Employee LIMIT 1;";

        var result = await checkCmd.ExecuteScalarAsync();
        if (result != null)
        {
            return;
        }

        var employees = new EmployeeEntity[]
        {
            new EmployeeEntity { Id = 1, Name = "Will", ManagerId = null, Enable = true },
            new EmployeeEntity { Id = 2, Name = "Bob", ManagerId = 1, Enable = false },
            new EmployeeEntity { Id = 3, Name = "Roman", ManagerId = 2, Enable = true },
            new EmployeeEntity { Id = 4, Name = "John", ManagerId = 3, Enable = true },
            new EmployeeEntity { Id = 5, Name = "Alice", ManagerId = 3, Enable = false },
            new EmployeeEntity { Id = 6, Name = "Julia", ManagerId = 2, Enable = true },
            new EmployeeEntity { Id = 7, Name = "Anna", ManagerId = 4, Enable = false }
        };

        foreach (var emp in employees)
        {
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = @"
                INSERT INTO Employee (Id, Name, ManagerId, Enable)
                VALUES (@id, @name, @managerId, @enable)
                ON CONFLICT (Id) DO NOTHING;
            ";

            insertCmd.Parameters.AddWithValue("id", emp.Id);
            insertCmd.Parameters.AddWithValue("name", emp.Name);
            insertCmd.Parameters.AddWithValue("managerId", (object?)emp.ManagerId ?? DBNull.Value);
            insertCmd.Parameters.AddWithValue("enable", emp.Enable);

            await insertCmd.ExecuteNonQueryAsync();
        }
    }
}
