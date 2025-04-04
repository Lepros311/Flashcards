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
                    CREATE TABLE IF NOT EXISTS Stacks (
                        StackID INT IDENTITY(1,1) PRIMARY KEY,
                        StackName NVARCHAR(100) NOT NULL
                    );";

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
    }
}
