using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Flashcards.Model
{
    public static class DatabaseUtility
    {
        // Method to count rows in a specified table
        public static int CountRows(string connectionString, string tableName)
        {
            // Validate the table name to prevent SQL injection
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));
            }

            // Optionally, maintain a list of valid table names to check against
            var validTables = new[] { "Stacks", "Flashcards", "StudySessionStats" }; // Add your valid table names here
            if (!Array.Exists(validTables, t => t.Equals(tableName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Invalid table name.", nameof(tableName));
            }

            // Prepare the SQL query
            string query = $"SELECT COUNT(1) FROM {tableName};";

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        // Method to check if a row with a specific ID exists in a specified table
        public static bool CheckIfIdExists(string connectionString, string tableName, int id)
        {
            // Validate the table name to prevent SQL injection
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));
            }

            // Optionally, maintain a list of valid table names to check against
            var validTables = new[] { "Stacks", "Flashcards", "StudySessionStats" }; // Add your valid table names here
            if (!Array.Exists(validTables, t => t.Equals(tableName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Invalid table name.", nameof(tableName));
            }

            string query = "";
            // Prepare the SQL query
            if (tableName == "Stacks")
            {
                query = $"SELECT COUNT(1) FROM Stacks WHERE StackId = @Id";
            }
            if (tableName == "Flashcards")
            {
                query = $"SELECT COUNT(1) FROM Flashcards WHERE FlashcardId = @Id";
            }

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", id));
                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        public static string GetConnectionString()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string projectDirectory = Path.Combine(currentDirectory, @"..\..\..");

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{projectDirectory}\\app.json", optional: true, reloadOnChange: true)
            .Build();

            string? connectionString = configuration.GetConnectionString("connection");

            return connectionString;
        }
    }
}
