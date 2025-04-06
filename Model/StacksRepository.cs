using Microsoft.Data.SqlClient;

namespace Flashcards.Model
{
    public class StacksRepository
    {
        private readonly string _connectionString;

        public StacksRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateTable()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks')
                    BEGIN
                        CREATE TABLE Stacks (
                            StackID INT IDENTITY(1,1) PRIMARY KEY,
                            StackName NVARCHAR(100) NOT NULL
                        );
                    END;";

                using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Stacks table created successfully.");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"An error occurred while creating the Stacks table: {ex.Message}");
                    }
                }
            }
        }

        public void SeedStacks()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string insertStacksQuery = "INSERT INTO Stacks (StackName) VALUES (@StackName); SELECT SCOPE_IDENTITY();";

                var stackNames = new List<string> { "Math", "Science", "History" };

                foreach (var stackName in stackNames)
                {
                    using (SqlCommand command = new SqlCommand(insertStacksQuery, connection))
                    {
                        command.Parameters.AddWithValue("@StackName", stackName);
                        var stackId = command.ExecuteScalar();
                        Console.WriteLine($"Inserted Stack: {stackName} with ID: {stackId}");
                    }
                }
            }
        }

        public List<Stacks> GetAllStacks()
        {
            var sessions = new List<Stacks>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"SELECT * FROM Stacks ORDER BY StackID DESC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var session = new Stacks
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        };
                        sessions.Add(session);
                    }
                }
            }

            return sessions;
        }

    }
}
